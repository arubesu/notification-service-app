using System.Collections.Concurrent;
using NotificationApp.Domain.RateLimits;

namespace NotificationApp.Domain.Interfaces;

public interface IRateLimitStrategy
{
    bool IsRateLimited(ConcurrentQueue<DateTime> requestQueue,
                       RateLimitRule rule,
                       DateTime currentTime);
    void LogRequest(ConcurrentQueue<DateTime> requestQueue,
                    DateTime currentTime);
}
