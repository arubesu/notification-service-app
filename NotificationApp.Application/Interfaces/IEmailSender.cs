using ErrorOr;

namespace NotificationApp.Application.Interfaces;

public interface IEmailSender
{
    Task<ErrorOr<bool>> SendEmailAsync(string userId, string message);
}
