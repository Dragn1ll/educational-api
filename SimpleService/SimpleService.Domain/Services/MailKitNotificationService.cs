using SimpleService.Domain.Abstractions;
using SimpleService.Infrastructure.Email;
using SimpleService.Infrastructure.Email.Abstractions;

namespace SimpleService.Domain.Services;

public class MailKitNotificationService : INotificationService
{
    private readonly IEmailService _emailService;

    public MailKitNotificationService(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task SendBulkEmailAsync(IEnumerable<EmailUser> users, string subject, string htmlTemplate, 
        int delayMilliseconds = 1000, CancellationToken cancellationToken = default)
    {
        foreach (var user in users)
        {
            var personalizedBody = htmlTemplate
                .Replace("{{Name}}", user.Name)
                .Replace("{{Email}}", user.Email);

            await _emailService.SendEmailAsync(user.Email, subject, personalizedBody, true, cancellationToken);

            if (delayMilliseconds > 0)
                await Task.Delay(delayMilliseconds, cancellationToken);
        }
    }
}