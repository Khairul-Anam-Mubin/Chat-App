using System.Reflection;
using Chat.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Contact.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddContactApplication(this IServiceCollection services)
    {
        services.AddHandlers(Assembly.GetExecutingAssembly());
        return services;
    }
}