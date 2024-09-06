using NotificationApp.Application.Helpers;
using NotificationApp.Application.Interfaces;
using NotificationApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace NotificationApp.Application.Extensions;

public static class ApplicationDependencyInjectionExtensions
{

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton(RateLimitStrategyRegistry.CreateStrategies());
        services.AddSingleton<IRateLimitService, RateLimitService>();

        services.AddTransient<INotificationService, NotificationService>();
        services.AddTransient<IEmailSender, EmailService>();

        return services;
    }
}
