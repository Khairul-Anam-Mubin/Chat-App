using Chat.Framework.Cache.DistributedCache;
using Chat.Framework.ORM.Factories;
using Chat.Framework.ORM.Interfaces;
using Chat.Framework.ORM.MongoDb;
using Chat.Framework.ORM.Sql;
using Chat.Framework.ORM.UnitOfWorks;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Framework.ORM;

public static class DependencyInjection
{
    public static IServiceCollection AddSqlDb(this IServiceCollection services)
    {
        services.AddSingleton<ISqlClientManager, SqlClientManager>();
        services.AddSingleton<SqlDbContext>();
        services.AddTransient<IDbContextFactory, DbContextFactory>();

        return services;
    }

    public static IServiceCollection AddMongoDb(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClientManager, MongoClientManager>();
        services.AddTransient<IIndexManager, MongoDbIndexManager>();
        services.AddSingleton<MongoDbContext>();
        services.AddTransient<IDbContextFactory, DbContextFactory>();
        services.AddSingleton<IUnitOfWork, UnitOfWork>();
        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services)
    {
        services.AddSingleton<IRedisClientManager, RedisClientManager>();
        services.AddSingleton<IDistributedCache, RedisCache>();
        services.AddTransient<IDbContextFactory, DbContextFactory>();
        return services;
    }
}