using Chat.Framework;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Database;
using Chat.Framework.Mediators;
using Chat.Framework.ServiceInstaller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Infrastructure.Shared.Configurations;

public class CommonServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAttributeRegisteredServices(AssemblyCache.Instance.GetAddedAssemblies());

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