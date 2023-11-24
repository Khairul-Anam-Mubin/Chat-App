using Chat.Activity.Application;
using Chat.Activity.Domain.Interfaces.Repositories;
using Chat.Activity.Infrastructure.Repositories;
using Chat.Framework.ServiceInstaller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Activity.Infrastructure.Configurations;

public class ActivityInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddActivityApplication()
            .AddSingleton<ILastSeenRepository, LastSeenRepository>();
    }
}