using NotificationApp.Application.Interfaces;
using NotificationApp.Domain.Enums;

namespace NotificationApp.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IRateLimitService _rateLimitService;
    private readonly IEmailSender _emailSender;

    public NotificationService(IRateLimitService rateLimitService,
                               IEmailSender emailSender)
    {
        _rateLimitService = rateLimitService;
        _emailSender = emailSender;
    }

    public async Task SendAsync(NotificationType type,
                                string userId,
                                string message)
    {
        if (await _rateLimitService.IsRateLimited(type, userId))
        {
            throw new InvalidOperationException("Rate limit exceeded");
        }

        await _emailSender.SendEmailAsync(userId, message);
    }
}
