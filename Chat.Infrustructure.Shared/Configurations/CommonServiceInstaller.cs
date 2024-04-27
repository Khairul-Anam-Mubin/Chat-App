﻿using KCluster.Framework;
using KCluster.Framework.Attributes;
using KCluster.Framework.CQRS;
using KCluster.Framework.EDD;
using KCluster.Framework.Extensions;
using KCluster.Framework.Identity;
using KCluster.Framework.Loggers;
using KCluster.Framework.Mediators;
using KCluster.Framework.ORM;
using KCluster  .Framework.ServiceInstaller;
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

        services.AddIdentityScope();

        services.AddFeatureManagement(configuration.GetSection("FeatureFlags"));

        services.AddMemoryCache();
        services.AddSqlDb();
        services.AddMongoDb();
        services.AddRedis();

        services.AddMediator();
        services.AddMediatR(cfg =>
        {
            AssemblyCache.Instance.GetAddedAssemblies()
            .ForEach(
                assembly => cfg.RegisterServicesFromAssembly(assembly));
        });

        services.AddTransient<IEventExecutor, EventExecutor>();
        services.AddTransient<IQueryExecutor, QueryExecutor>();
        services.AddTransient<ICommandExecutor, CommandExecutor>();
        services.AddTransient<ICommandService, CommandService>();
        services.AddTransient<IQueryService, QueryService>();
        services.AddTransient<IEventService, EventService>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHttpClient();
        services.AddHttpContextAccessor();
    }
}