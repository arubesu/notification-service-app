using NotificationApp.Domain.Enums;

namespace NotificationApp.Application.Models;

public class NotificationRequest
{
    public NotificationType Type { get; set; }
    public required string UserId { get; set; }
    public required string Message { get; set; }
}
