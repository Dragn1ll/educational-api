using SimpleService.Infrastructure.Email;

namespace SimpleService.Domain.Abstractions;

public interface INotificationService
{
    Task SendBulkEmailAsync(IEnumerable<EmailUser> users, string subject, string htmlTemplate,
        int delayMilliseconds = 1000, CancellationToken cancellationToken = default);
}