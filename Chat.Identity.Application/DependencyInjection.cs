using Chat.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Chat.Identity.Application.Services;
using Chat.Identity.Domain.Interfaces;

namespace Chat.Identity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityApplication(this IServiceCollection services)
    {
        services.AddSingleton<ITokenService, TokenService>();
        services.AddHandlers(Assembly.GetExecutingAssembly());
        return services;
    }
}