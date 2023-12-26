using System.Reflection;
using Chat.Framework.EmailSenders;
using Chat.Framework.Mediators;
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