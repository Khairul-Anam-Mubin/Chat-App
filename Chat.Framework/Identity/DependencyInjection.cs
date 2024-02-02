using Microsoft.Extensions.DependencyInjection;

namespace Chat.Framework.Identity;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityScope(this IServiceCollection services)
    {
        services.AddTransient<IdentityMiddleware>();
        services.AddScoped<IScopeIdentity, ScopeIdentity>();
        return services;
    }
}
