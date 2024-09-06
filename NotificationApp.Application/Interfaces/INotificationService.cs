using NotificationApp.Domain.Enums;

namespace NotificationApp.Application.Interfaces;

public interface INotificationService
{
    Task SendAsync(NotificationType type, string userId, string message);
}
