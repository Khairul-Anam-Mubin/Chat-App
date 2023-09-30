using Microsoft.Extensions.DependencyInjection;

namespace Chat.Activity.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddActivityDomain(this IServiceCollection services)
    {
        return services;
    }
}