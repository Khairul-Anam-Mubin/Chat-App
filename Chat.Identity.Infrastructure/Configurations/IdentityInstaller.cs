using KCluster.Framework.ORM.Interfaces;
using KCluster.Framework.ServiceInstaller;
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
        services.AddTransient<IMigrationJob, PermissionMigrationJob>();
        services.AddTransient<IMigrationJob, RoleMigrationJob>();
        
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserAccessRepository, UserAccessRepository>();

        services.AddScoped<IAccessService, AccessService>();
        services.AddScoped<ITokenService, TokenService>();
    }
}