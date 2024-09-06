using ErrorOr;
using NotificationApp.Domain.Enums;

namespace NotificationApp.Application.Interfaces;

public interface INotificationService
{
    Task<ErrorOr<bool>> SendAsync(NotificationType type, string userId, string message);
}
