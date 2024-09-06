using NotificationApp.Application.Interfaces;

namespace NotificationApp.Application.Services;

public class EmailService : IEmailSender
{
    public Task SendEmailAsync(string userId, string message)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Email content cannot be null or empty.", nameof(message));

        Console.WriteLine($"Sending email to {userId}: {message}");

        return Task.CompletedTask;
    }
}
