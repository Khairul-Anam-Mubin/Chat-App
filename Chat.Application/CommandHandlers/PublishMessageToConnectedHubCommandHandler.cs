using Chat.Application.Interfaces;
using Chat.Domain.Commands;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Extensions;
using Chat.Framework.Mediators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<PublishMessageToConnectedHubCommand>), ServiceLifetime.Singleton)]
public class PublishMessageToConnectedHubCommandHandler : ICommandHandler<PublishMessageToConnectedHubCommand, CommandResponse>
{
    private readonly IConfiguration _configuration;
    private readonly IRedisContext _redisContext;
    private readonly IHubConnectionService _hubConnectionService;

    public PublishMessageToConnectedHubCommandHandler(
        IRedisContext redisContext,
        IConfiguration configuration,
        IHubConnectionService hubConnectionService)
    {
        _redisContext = redisContext;
        _configuration = configuration;
        _hubConnectionService = hubConnectionService;
    }

    public async Task<CommandResponse> HandleAsync(PublishMessageToConnectedHubCommand command)
    {
        var subscriber = _redisContext.GetSubscriber(
            _configuration.GetSection("RedisConfig:DatabaseInfo").Get<DatabaseInfo>());

        var channel = await _hubConnectionService.GetUserConnectedHubInstanceIdAsync(command.SendTo);

        if (string.IsNullOrEmpty(channel))
        {
            Console.WriteLine($"Channel not found. So publish event to redis skipped\n");
            return command.CreateResponse();
        }

        await subscriber.PublishAsync(channel, new PubSubMessage
        {
            Id = command.MessageId,
            MessageType = MessageType.UserMessage,
            Message = "Publish"
        }.Serialize());

        Console.WriteLine("Event published to redis\n");
        return command.CreateResponse();
    }
}