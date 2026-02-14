using System.Net.Mail;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SimpleService.Infrastructure.Email.Abstractions;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace SimpleService.Infrastructure.Email;

public class MailKitEmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly ILogger<MailKitEmailService> _logger;

    public MailKitEmailService(IOptions<EmailSettings> settings, ILogger<MailKitEmailService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true,
        CancellationToken cancellationToken = default)
        => await SendEmailAsync(to, subject, body, attachments: null, isHtml, cancellationToken);

    public async Task SendEmailAsync(string to, string subject, string body, IEnumerable<Attachment>? attachments = null, 
        bool isHtml = true, CancellationToken cancellationToken = default)
    {
        var message = CreateMimeMessage(to, subject, body, attachments, isHtml);
        await SendWithRetryAsync(message, cancellationToken);
    }

    private MimeMessage CreateMimeMessage(string to, string subject, string body, IEnumerable<Attachment>? attachments, 
        bool isHtml)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder();
        if (isHtml)
            bodyBuilder.HtmlBody = body;
        else
            bodyBuilder.TextBody = body;

        if (attachments != null)
        {
            foreach (var attachment in attachments)
            {
                bodyBuilder.Attachments.Add(attachment.Name, attachment.ContentStream);
            }
        }

        message.Body = bodyBuilder.ToMessageBody();
        return message;
    }

    private async Task SendWithRetryAsync(MimeMessage message, CancellationToken cancellationToken)
    {
        var retryCount = 0;
        const int maxRetries = 3;
        const int delayMs = 1000;

        while (retryCount <= maxRetries)
        {
            try
            {
                using var client = new SmtpClient();
                client.ServerCertificateValidationCallback = (_, _, _, _) => true;

                await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort,
                    _settings.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto, cancellationToken);

                if (!string.IsNullOrEmpty(_settings.UserName))
                {
                    await client.AuthenticateAsync(_settings.UserName, _settings.Password, cancellationToken);
                }

                await client.SendAsync(message, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);

                _logger.LogInformation("Email sent successfully to {To}", message.To);
                return;
            }
            catch (Exception ex) when (retryCount < maxRetries)
            {
                retryCount++;
                _logger.LogWarning(ex, "Failed to send email to {To}, retry {Retry}/{MaxRetries}", message.To, 
                    retryCount, maxRetries);
                await Task.Delay(delayMs * retryCount, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To} after {MaxRetries} attempts", message.To, 
                    maxRetries);
                throw;
            }
        }
    }
}