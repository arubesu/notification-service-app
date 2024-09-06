using NotificationApp.Domain.Enums;

namespace NotificationApp.Domain.Entities;

public class Notification
{
    public NotificationType Type { get; set; }
    public required string UserId { get; set; }
    public required string Message { get; set; }
}
