using System.Reflection;
using KCluster.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Activity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddActivityApplication(this IServiceCollection services)
    {
        services.AddHandlers(Assembly.GetExecutingAssembly());
        return services;
    }
}