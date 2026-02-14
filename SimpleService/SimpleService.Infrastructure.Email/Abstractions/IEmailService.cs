using System.Net.Mail;

namespace SimpleService.Infrastructure.Email.Abstractions;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = true,
        CancellationToken cancellationToken = default);

    Task SendEmailAsync(string to, string subject, string body, IEnumerable<Attachment>? attachments = null,
        bool isHtml = true, CancellationToken cancellationToken = default);
}