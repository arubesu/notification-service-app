using System.Collections.Concurrent;
using NotificationApp.Application.Interfaces;
using NotificationApp.Domain.Enums;
using NotificationApp.Domain.Interfaces;
using NotificationApp.Domain.RateLimits;

namespace NotificationApp.Application.Services;

public class RateLimitService : IRateLimitService
{
    private readonly ConcurrentDictionary<(NotificationType, string), ConcurrentQueue<DateTime>> _requestLogs = new();
    private readonly ConcurrentDictionary<NotificationType, IRateLimitStrategy> _strategies;

    public RateLimitService(
        ConcurrentDictionary<NotificationType, IRateLimitStrategy> strategies)
    {
        _strategies = strategies;
    }

    public Task<bool> IsRateLimited(NotificationType type, string userId)
    {
        return Task.Run(() => CheckRateLimit(type, userId));
    }

    private bool CheckRateLimit(NotificationType type, string userId)
    {
        var rule = GetRateLimitRule(type);
        var strategy = GetRateLimitStrategy(type);
        var requestQueue = GetOrCreateRequestQueue(type, userId);
        var now = DateTime.UtcNow;

        if (strategy.IsRateLimited(requestQueue, rule, now))
        {
            return true;
        }

        strategy.LogRequest(requestQueue, now);
        return false;
    }

    private RateLimitRule GetRateLimitRule(NotificationType type)
    {
        if (!RateLimitRules.Rules.TryGetValue(type, out var rule))
        {
            throw new ArgumentException("Unknown notification type", nameof(type));
        }
        return rule;
    }

    private IRateLimitStrategy GetRateLimitStrategy(NotificationType type)
    {
        if (!_strategies.TryGetValue(type, out var strategy))
        {
            throw new ArgumentException("Unknown notification type", nameof(type));
        }
        return strategy;
    }

    private ConcurrentQueue<DateTime> GetOrCreateRequestQueue(NotificationType type, string userId)
    {
        var key = (type, userId);
        return _requestLogs.GetOrAdd(key, _ => new ConcurrentQueue<DateTime>());
    }
}
