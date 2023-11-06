using Microsoft.Extensions.DependencyInjection;

namespace Chat.Notification.Infrastructure;

public static class ServiceConfiguration
{
    public static IServiceCollection AddNotifications(this IServiceCollection services)
    {
        services.AddSignalR();
        return services;
    }
}