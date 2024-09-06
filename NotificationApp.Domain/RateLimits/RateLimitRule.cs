using NotificationApp.Domain.Enums;

namespace NotificationApp.Domain.RateLimits;

public class RateLimitRule
{
    public TimeSpan Period { get; set; }
    public int MaxRequests { get; set; }

}

public static class RateLimitRules
{
    public static readonly Dictionary<NotificationType, RateLimitRule> Rules = new()
        {
            { NotificationType.StatusUpdate, new RateLimitRule { Period = TimeSpan.FromMinutes(1), MaxRequests = 2 } },
            { NotificationType.DailyNews, new RateLimitRule { Period = TimeSpan.Zero, MaxRequests = 1 } },
            { NotificationType.Marketing, new RateLimitRule { Period = TimeSpan.FromHours(1), MaxRequests = 3 } }
        };
}
