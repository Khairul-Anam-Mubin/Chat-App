using Chat.Framework.Attributes;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Mediators;
using Chat.Framework.Models;
using Chat.Notification.Domain.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Notification.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<PublishNotificationToConnectedHubCommand, Response>), ServiceLifetime.Transient)]
public class PublishNotificationToConnectedHubCommandHandler :
    IRequestHandler<PublishNotificationToConnectedHubCommand, Response>
{
    private readonly IRedisContext _redisContext;
    private readonly DatabaseInfo _databaseInfo;

    public PublishNotificationToConnectedHubCommandHandler(
        IConfiguration configuration,
        IRedisContext redisContext)
    {
        _redisContext = redisContext;
        _databaseInfo = configuration.GetSection("RedisConfig:DatabaseInfo")
            .Get<DatabaseInfo>();
    }

    public async Task<Response> HandleAsync(PublishNotificationToConnectedHubCommand request)
    {
        var channel = request.HubInstanceId;

        if (string.IsNullOrEmpty(channel))
        {
            Console.WriteLine($"Channel not found. So publish event to redis skipped\n");
            return Response.Create();
        }

        var pubSubMessage = new PubSubMessage
        {
            Id = request.Notification!.Id,
            Message = new SendNotificationToClientCommand
            {
                Notification = request.Notification,
                ReceiverIds = request.ReceiverIds,
            },
            MessageType = MessageType.Notification
        };

        await _redisContext.PublishAsync(_databaseInfo, channel, pubSubMessage);

        Console.WriteLine("Event published to redis\n");

        return Response.Create();
    }
}