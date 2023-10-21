using Chat.Application.Interfaces;
using Chat.Domain.Events;
using Chat.Framework.Attributes;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Extensions;
using Chat.Framework.Mediators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.EventHandlers;

[ServiceRegister(typeof(IRequestHandler<PublishMessageToConnectedHubEvent>), ServiceLifetime.Singleton)]
public class PublishMessageToConnectedHubEventHandler : IRequestHandler<PublishMessageToConnectedHubEvent>
{
    private readonly IConfiguration _configuration;
    private readonly IRedisContext _redisContext;
    private readonly IHubConnectionService _hubConnectionService;

    public PublishMessageToConnectedHubEventHandler(
        IRedisContext redisContext,
        IConfiguration configuration, 
        IHubConnectionService hubConnectionService)
    {
        _redisContext = redisContext;
        _configuration = configuration;
        _hubConnectionService = hubConnectionService;
    }

    public async Task HandleAsync(PublishMessageToConnectedHubEvent request)
    {
        var subscriber = _redisContext.GetSubscriber(
            _configuration.GetSection("RedisConfig:DatabaseInfo").Get<DatabaseInfo>());

        var channel = await _hubConnectionService.GetUserConnectedHubInstanceIdAsync(request.SendTo);

        if (string.IsNullOrEmpty(channel))
        {
            Console.WriteLine($"Channel not found. So publish event to redis skipped\n");
            return;
        }

        await subscriber.PublishAsync(channel, new PubSubMessage
        {
            Id = request.MessageId,
            MessageType = MessageType.UserMessage,
            Message = "Publish"
        }.Serialize());

        Console.WriteLine("Event published to redis\n");
    }
}