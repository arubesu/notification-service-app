using System;
using System.Collections.Concurrent;
using NotificationApp.Domain.Enums;
using NotificationApp.Domain.Interfaces;
using NotificationApp.Domain.RateLimits;

namespace NotificationApp.Application.Helpers;

public class RateLimitStrategyRegistry
{
    public static ConcurrentDictionary<NotificationType, IRateLimitStrategy> CreateStrategies()
    {
        var timeBasedRateLimitStrategy = new TimeBasedRateLimitStrategy();
        var recipientCountBasedRateLimitStrategy = new RecipientCountBasedRateLimitStrategy();

        var strategies = new ConcurrentDictionary<NotificationType, IRateLimitStrategy>
        {
            [NotificationType.StatusUpdate] = timeBasedRateLimitStrategy,
            [NotificationType.DailyNews] = recipientCountBasedRateLimitStrategy,
            [NotificationType.Marketing] = timeBasedRateLimitStrategy
        };

        return strategies;
    }
}
