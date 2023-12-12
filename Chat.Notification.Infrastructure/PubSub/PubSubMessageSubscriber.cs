using Chat.Framework.CQRS;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Database.ORM;
using Chat.Framework.Extensions;
using Chat.Framework.Interfaces;
using Chat.Framework.Mediators;
using Chat.Framework.Results;
using Chat.Notification.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Chat.Notification.Infrastructure.PubSub;

public sealed class PubSubMessageSubscriber : IInitialService
{
    private readonly IHubConnectionService _hubConnectionService;
    private readonly IRedisContext _redisContext;
    private readonly IConfiguration _configuration;
    private readonly ICommandExecutor _commandExecutor;
    private readonly IMediator _mediator;

    public PubSubMessageSubscriber(
        IRedisContext redisContext,
        IConfiguration configuration,
        ICommandExecutor commandExecutor,
        IHubConnectionService hubConnectionService, IMediator mediator)
    {
        _redisContext = redisContext;
        _configuration = configuration;
        _commandExecutor = commandExecutor;
        _hubConnectionService = hubConnectionService;
        _mediator = mediator;
    }

    public async Task InitializeAsync()
    {
        var subscriber = _redisContext.GetSubscriber(_configuration.GetConfig<DatabaseInfo>("RedisConfig")!);

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

            Task.Factory.StartNew(() => _mediator.SendAsync<PubSubMessage, IResult>(pubSubMessage!));

        });
    }
}