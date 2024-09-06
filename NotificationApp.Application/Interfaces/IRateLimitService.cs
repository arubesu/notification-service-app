using NotificationApp.Domain.Enums;

namespace NotificationApp.Application.Interfaces;

public interface IRateLimitService
{
    Task<bool> IsRateLimited(NotificationType type, string userId);
}
