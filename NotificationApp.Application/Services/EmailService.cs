using ErrorOr;
using NotificationApp.Application.Interfaces;

namespace NotificationApp.Application.Services;

public class EmailService : IEmailSender
{
    public async Task<ErrorOr<bool>> SendEmailAsync(string userId, string message)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Error.Validation("InvalidUserId", "User ID cannot be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            return Error.Validation("InvalidMessage", "Email content cannot be null or empty.");
        }
        await Task.Run(() =>
      {
          Console.WriteLine($"Sending email to {userId}: {message}");
      });

        return true;
    }
}
