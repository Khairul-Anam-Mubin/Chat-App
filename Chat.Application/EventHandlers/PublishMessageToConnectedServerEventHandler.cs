using Chat.Application.Interfaces;
using Chat.Domain.Events;
using Chat.Framework.Attributes;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Extensions;
using Chat.Framework.Mediators;
using Chat.Framework.Proxy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.EventHandlers;

[ServiceRegister(typeof(IRequestHandler<PublishMessageToConnectedServerEvent>), ServiceLifetime.Singleton)]
public class PublishMessageToConnectedServerEventHandler : IRequestHandler<PublishMessageToConnectedServerEvent>
{
    private readonly IConfiguration _configuration;
    private readonly IRedisContext _redisContext;

    public PublishMessageToConnectedServerEventHandler(
        IRedisContext redisContext,
        IConfiguration configuration)
    {
        _redisContext = redisContext;
        _configuration = configuration;
    }

    public async Task HandleAsync(PublishMessageToConnectedServerEvent request)
    {
        var subscriber = _redisContext.GetSubscriber(
            _configuration.GetSection("RedisConfig:DatabaseInfo").Get<DatabaseInfo>());

        var channel = _configuration.GetSection("RedisConfig:GlobalChannel").Get<string>();
        // Todo: will get the channel from user session persistent store

        await subscriber.PublishAsync(channel, new PubSubMessage
        {
            Id = request.MessageId,
            MessageType = MessageType.UserMessage,
            Message = "Publish"
        }.Serialize());
    }
}