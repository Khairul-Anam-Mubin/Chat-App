using Peacious.Framework.Interfaces;
using Peacious.Framework.PubSub;
using Peacious.Framework.ServiceInstaller;
using Chat.Notification.Application;
using Chat.Notification.Domain.Interfaces;
using Chat.Notification.Infrastructure.PubSub;
using Chat.Notification.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Notification.Infrastructure.Configurations;

public class NotificationInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddNotificationApplication();
        services.AddSignalR();
        services.AddSingleton<IHubConnectionService, HubConnectionService>();
        services.AddSingleton<INotificationHubService, NotificationHubService>();
        services.AddSingleton<IInitialService, PubSubMessageSubscriber>();
        services.AddPubSub();
    }
}