using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Framework.Identity;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityScope(this IServiceCollection services)
    {
        services.AddTransient<IdentityMiddleware>();
        services.AddScoped<IScopeIdentity, ScopeIdentity>();

        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermisisonAuthorizationPolicyProvider>();

        return services;
    }
}
