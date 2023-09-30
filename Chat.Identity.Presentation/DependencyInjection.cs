using Microsoft.Extensions.DependencyInjection;

namespace Chat.Identity.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddActivityDomain(this IServiceCollection services)
        {
            return services;
        }
    }
}