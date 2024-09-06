using System.Collections.Concurrent;
using NotificationApp.Domain.Interfaces;

namespace NotificationApp.Domain.RateLimits;

public class RecipientCountBasedRateLimitStrategy : IRateLimitStrategy
{
    public bool IsRateLimited(ConcurrentQueue<DateTime> requestQueue,
                              RateLimitRule rule,
                              DateTime currentTime)
    {
        return requestQueue.Count >= rule.MaxRequests;
    }

    public void LogRequest(ConcurrentQueue<DateTime> requestQueue,
                           DateTime currentTime)
    {
        requestQueue.Enqueue(currentTime);
    }
}
