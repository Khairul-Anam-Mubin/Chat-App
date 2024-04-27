using System.Reflection;
using KCluster.Framework.EmailSenders;
using KCluster.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Notification.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddNotificationApplication(this IServiceCollection services)
    {
        services.AddHandlers(Assembly.GetExecutingAssembly());
        services.AddSingleton<IEmailSender, SmtpEmailSender>();
        return services;
    }
}