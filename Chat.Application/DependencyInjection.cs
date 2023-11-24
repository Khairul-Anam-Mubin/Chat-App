using System.Reflection;
using Chat.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddChatApplication(this IServiceCollection services)
    {
        services.AddHandlers(Assembly.GetExecutingAssembly());
        return services;
    }
}