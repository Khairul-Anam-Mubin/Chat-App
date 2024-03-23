using Chat.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Chat.Identity.Domain.Services;

namespace Chat.Identity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityApplication(this IServiceCollection services)
    {
        services.AddHandlers(Assembly.GetExecutingAssembly());
        return services;
    }
}