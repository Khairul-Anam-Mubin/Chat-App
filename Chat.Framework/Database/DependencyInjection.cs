using Chat.Framework.Database.Clients;
using Chat.Framework.Database.Contexts;
using Chat.Framework.Database.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Framework.Database;

public static class DependencyInjection
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClientManager, MongoClientManager>();
        services.AddSingleton<IMongoDbContext, MongoDbContext>();
        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services)
    {
        services.AddSingleton<IRedisClientManager, RedisClientManager>();
        services.AddSingleton<IRedisContext, RedisContext>();
        return services;
    }
}