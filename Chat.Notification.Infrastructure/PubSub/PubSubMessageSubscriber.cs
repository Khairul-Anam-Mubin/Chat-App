using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Extensions;
using Chat.Framework.Interfaces;
using Chat.Notification.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Notification.Infrastructure.PubSub;

[ServiceRegister(typeof(IInitialService), ServiceLifetime.Singleton)]
public sealed class PubSubMessageSubscriber : IInitialService
{
    private readonly IHubConnectionService _hubConnectionService;
    private readonly IRedisContext _redisContext;
    private readonly IConfiguration _configuration;
    private readonly ICommandService _commandService;

    public PubSubMessageSubscriber(
        IRedisContext redisContext,
        IConfiguration configuration,
        ICommandService commandService,
        IHubConnectionService hubConnectionService)
    {
        _redisContext = redisContext;
        _configuration = configuration;
        _commandService = commandService;
        _hubConnectionService = hubConnectionService;
    }

    public async Task InitializeAsync()
    {
        var subscriber = _redisContext.GetSubscriber(
            _configuration.GetSection("RedisConfig:DatabaseInfo").Get<DatabaseInfo>());

        var channel = _hubConnectionService.GetCurrentHubInstanceId();

        Console.WriteLine($"Subscribing to redis channel : {channel}\n");

        await subscriber.SubscribeAsync(channel, (redisChannel, message) =>
        {
            Console.WriteLine($"Content received form channel : {redisChannel}\n");
            var pubSubMessage = message.SmartCast<PubSubMessage>();

            if (pubSubMessage == null)
            {
                Console.WriteLine("Message is null here");
                return;
            }

            Console.WriteLine($"PubSubMessage.Id : {pubSubMessage?.Id}, PubSubMessageType: {pubSubMessage?.MessageType.ToString()} , message : {message}\n");

            Task.Factory.StartNew(() => _commandService.GetResponseAsync(pubSubMessage!));

        });
    }
}