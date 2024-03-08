using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.ServiceInstaller;
using Chat.Identity.Application;
using Chat.Identity.Domain.Repositories;
using Chat.Identity.Infrastructure.Migrations;
using Chat.Identity.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Identity.Infrastructure.Configurations;

public class IdentityInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityApplication();

        services.AddTransient<IIndexCreator, UserModelIndexCreator>();
        services.AddTransient<IIndexCreator, AccessModelIndexCreator>();
        services.AddSingleton<IAccessRepository, AccessRepository>();
        services.AddSingleton<IUserRepository, UserRepository>();
    }
}