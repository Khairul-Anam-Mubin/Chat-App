using Peacious.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Chat.Identity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityApplication(this IServiceCollection services)
    {
        services.AddHandlers(Assembly.GetExecutingAssembly());
        return services;
    }
}