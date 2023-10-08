using Microsoft.Extensions.DependencyInjection;

namespace Chat.Identity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddActivityDomain(this IServiceCollection services)
    {
        return services;
    }
}