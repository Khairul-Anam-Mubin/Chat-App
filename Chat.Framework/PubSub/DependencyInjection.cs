using Chat.Framework.Cache.DistributedCache;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Framework.PubSub;

public static class DependencyInjection
{
    public static IServiceCollection AddPubSub(this IServiceCollection services)
    {
        services.AddSingleton<IRedisClientManager, RedisClientManager>();
        services.AddSingleton<IPubSub, RedisPubSub>();
        return services;
    }
}