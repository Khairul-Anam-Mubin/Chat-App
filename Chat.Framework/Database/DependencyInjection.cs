using Chat.Framework.Database.Clients;
using Chat.Framework.Database.Contexts;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.ORM.Factories;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Database.ORM.MongoDb;
using Chat.Framework.Database.ORM.Sql;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Framework.Database;

public static class DependencyInjection
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClientManager, MongoClientManager>();
        services.AddTransient<IIndexManager, MongoDbIndexManager>();
        services.AddSingleton<MongoDbContext>();
        services.AddSingleton<SqlDbContext>();
        services.AddTransient<IDbContextFactory, DbContextFactory>();
        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services)
    {
        services.AddSingleton<IRedisClientManager, RedisClientManager>();
        services.AddSingleton<IRedisContext, RedisContext>();
        services.AddTransient<IDbContextFactory, DbContextFactory>();
        return services;
    }
}