using Chat.Framework;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Database;
using Chat.Framework.Database.ORM;
using Chat.Framework.Extensions;
using Chat.Framework.Loggers;
using Chat.Framework.Mediators;
using Chat.Framework.ServiceInstaller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace Chat.Infrastructure.Shared.Configurations;

public class CommonServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAttributeRegisteredServices(AssemblyCache.Instance.GetAddedAssemblies());

        services.AddSingleton(configuration.TryGetConfig<DatabaseInfo>());

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddLogging(configuration);

        services.AddFeatureManagement(configuration.GetSection("FeatureFlags"));

        services.AddSqlDb();
        services.AddMongoDb();
        services.AddRedis();

        services.AddMediator();

        services.AddTransient<IQueryExecutor, QueryExecutor>();
        services.AddTransient<ICommandExecutor, CommandExecutor>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHttpClient();
        services.AddHttpContextAccessor();
    }
}