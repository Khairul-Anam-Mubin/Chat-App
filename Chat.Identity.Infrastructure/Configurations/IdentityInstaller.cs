using Chat.Framework.ORM.Interfaces;
using Chat.Framework.ServiceInstaller;
using Chat.Identity.Application;
using Chat.Identity.Domain.Repositories;
using Chat.Identity.Domain.Services;
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

        services.AddTransient<IIndexCreator, UserIndexCreator>();
        services.AddTransient<IIndexCreator, TokenIndexCreator>();
        
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccessRepository, AccessRepository>();

        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<IAccessService, AccessService>();
    }
}