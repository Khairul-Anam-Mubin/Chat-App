using Microsoft.Extensions.DependencyInjection;

namespace Chat.Activity.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddActivityPersistence(this IServiceCollection services)
    {
        return services;
    }
}