using System.Collections.Concurrent;
using NotificationApp.Application.Interfaces;
using NotificationApp.Application.Services;
using NotificationApp.Domain.Enums;
using NotificationApp.Domain.Interfaces;
using NotificationApp.Domain.RateLimits;
using NSubstitute;

namespace NotificationApp.Tests.Application.Services;

public class RateLimitServiceTests
{
    private IRateLimitService _rateLimitService;
    private readonly IRateLimitStrategy _timeBasedStrategy;
    private readonly IRateLimitStrategy _recipientCountBasedStrategy;
    private readonly ConcurrentDictionary<NotificationType, IRateLimitStrategy> _strategies;

    public RateLimitServiceTests()
    {
        _timeBasedStrategy = new TimeBasedRateLimitStrategy();
        _recipientCountBasedStrategy = new RecipientCountBasedRateLimitStrategy();

        _strategies = new ConcurrentDictionary<NotificationType, IRateLimitStrategy>
        {
            [NotificationType.StatusUpdate] = _timeBasedStrategy,
            [NotificationType.DailyNews] = _recipientCountBasedStrategy,
            [NotificationType.Marketing] = _timeBasedStrategy
        };

    }

    [Fact]
    public async Task StatusUpdates_LessThanOrEqualToTwoPerMinute_IsAllowed()
    {
        // Arrange
        _rateLimitService = new RateLimitService(_strategies);
        var notificationType = NotificationType.StatusUpdate;
        var userId = "user1";

        // Act
        var result1 = await _rateLimitService.IsRateLimited(notificationType, userId);
        var result2 = await _rateLimitService.IsRateLimited(notificationType, userId);

        // Assert
        Assert.False(result1);
        Assert.False(result2);
    }

    [Fact]
    public async Task StatusUpdates_MoreThanTwoPerMinute_IsNotAllowed()
    {
        // Arrange
        _rateLimitService = new RateLimitService(_strategies);
        var notificationType = NotificationType.StatusUpdate;
        var userId = "user1";

        // Act
        var result1 = await _rateLimitService.IsRateLimited(notificationType, userId);
        var result2 = await _rateLimitService.IsRateLimited(notificationType, userId);
        var result3 = await _rateLimitService.IsRateLimited(notificationType, userId);

        // Assert
        Assert.False(result1);
        Assert.False(result2);
        Assert.True(result3);
    }

    [Fact]
    public async Task NewsUpdates_LessThanOrEqualToOnePerDay_IsAllowed()
    {
        // Arrange
        _rateLimitService = new RateLimitService(_strategies);
        var notificationType = NotificationType.DailyNews;
        var userId = "user1";

        // Act
        var result1 = await _rateLimitService.IsRateLimited(notificationType, userId);

        // Assert
        Assert.False(result1);
    }

    [Fact]
    public async Task NewsUpdates_MoreThanOnePerDay_IsNotAllowed()
    {
        // Arrange
        _rateLimitService = new RateLimitService(_strategies);
        var notificationType = NotificationType.DailyNews;
        var userId = "user1";

        // Act
        var result1 = await _rateLimitService.IsRateLimited(notificationType, userId);
        var result2 = await _rateLimitService.IsRateLimited(notificationType, userId);

        // Assert
        Assert.False(result1);
        Assert.True(result2);
    }

    [Fact]
    public async Task MarketingUpdates_LessThanOrEqualToThreePerHour_IsAllowed()
    {
        // Arrange
        _rateLimitService = new RateLimitService(_strategies);
        var notificationType = NotificationType.Marketing;
        var userId = "user1";

        // Act
        var result1 = await _rateLimitService.IsRateLimited(notificationType, userId);
        var result2 = await _rateLimitService.IsRateLimited(notificationType, userId);
        var result3 = await _rateLimitService.IsRateLimited(notificationType, userId);

        // Assert
        Assert.False(result1);
        Assert.False(result2);
        Assert.False(result3);
    }

    [Fact]
    public async Task MarketingUpdates_MoreThanThreePerHour_IsNotAllowed()
    {
        // Arrange
        _rateLimitService = new RateLimitService(_strategies);

        var notificationType = NotificationType.Marketing;
        var userId = "user1";

        // Act
        var result1 = await _rateLimitService.IsRateLimited(notificationType, userId);
        var result2 = await _rateLimitService.IsRateLimited(notificationType, userId);
        var result3 = await _rateLimitService.IsRateLimited(notificationType, userId);
        var result4 = await _rateLimitService.IsRateLimited(notificationType, userId);

        // Assert
        Assert.False(result1);
        Assert.False(result2);
        Assert.False(result3);
        Assert.True(result4);
    }

    [Theory]
    [InlineData(NotificationType.StatusUpdate)]
    [InlineData(NotificationType.DailyNews)]
    [InlineData(NotificationType.Marketing)]
    public async Task RateLimits_AppliedToMultipleUsers_ShouldNotBeRateLimited(NotificationType notificationType)
    {
        // Arrange
        _rateLimitService = new RateLimitService(_strategies);

        // Act
        var result1 = await _rateLimitService.IsRateLimited(notificationType, "user1");
        var result2 = await _rateLimitService.IsRateLimited(notificationType, "user2");
        var result3 = await _rateLimitService.IsRateLimited(notificationType, "user3");
        var result4 = await _rateLimitService.IsRateLimited(notificationType, "user4");
        var result5 = await _rateLimitService.IsRateLimited(notificationType, "user5");
        var result6 = await _rateLimitService.IsRateLimited(notificationType, "user6");

        // Assert
        Assert.False(result1);
        Assert.False(result2);
        Assert.False(result3);
        Assert.False(result4);
        Assert.False(result5);
        Assert.False(result6);
    }
}