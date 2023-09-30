using Microsoft.Extensions.DependencyInjection;

namespace Chat.Activity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddActivityApplication(this IServiceCollection services)
    {
        return services;
    }
}