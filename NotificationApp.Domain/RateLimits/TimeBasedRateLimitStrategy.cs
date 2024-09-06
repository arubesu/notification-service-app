using System;
using System.Collections.Concurrent;
using NotificationApp.Domain.Interfaces;

namespace NotificationApp.Domain.RateLimits;

public class TimeBasedRateLimitStrategy : IRateLimitStrategy
{
    public bool IsRateLimited(ConcurrentQueue<DateTime> requestQueue,
                              RateLimitRule rule,
                              DateTime currentTime)
    {
        CleanOldRequests(requestQueue, rule, currentTime);

        return requestQueue.Count >= rule.MaxRequests;
    }

    private static void CleanOldRequests(ConcurrentQueue<DateTime> requestQueue,
                                         RateLimitRule rule,
                                         DateTime currentTime)
    {
        while (requestQueue.TryPeek(out var timestamp) && currentTime - timestamp > rule.Period)
        {
            requestQueue.TryDequeue(out _);
        }
    }

    public void LogRequest(ConcurrentQueue<DateTime> requestQueue, DateTime currentTime)
    {
        requestQueue.Enqueue(currentTime);
    }
}
