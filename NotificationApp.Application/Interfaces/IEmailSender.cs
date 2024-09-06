namespace NotificationApp.Application.Interfaces;

public interface IEmailSender
{
    Task SendEmailAsync(string userId, string message);
}
