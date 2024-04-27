using System.Reflection;
using KCluster.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.FileStore.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFileStoreApplication(this IServiceCollection services)
    {
        services.AddHandlers(Assembly.GetExecutingAssembly());
        return services;
    }
}