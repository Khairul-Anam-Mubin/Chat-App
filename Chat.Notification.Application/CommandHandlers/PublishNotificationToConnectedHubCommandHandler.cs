using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Database.ORM;
using Chat.Framework.Extensions;
using Chat.Framework.Mediators;
using Chat.Framework.RequestResponse;
using Chat.Notification.Application.Commands;
using Microsoft.Extensions.Configuration;

namespace Chat.Notification.Application.CommandHandlers;

public class PublishNotificationToConnectedHubCommandHandler :
    IHandler<PublishNotificationToConnectedHubCommand, IResponse>
{
    private readonly IRedisContext _redisContext;
    private readonly DatabaseInfo _databaseInfo;

    public PublishNotificationToConnectedHubCommandHandler(
        IConfiguration configuration,
        IRedisContext redisContext)
    {
        _redisContext = redisContext;
        _databaseInfo = configuration.GetConfig<DatabaseInfo>("RedisConfig")!;
    }

    public async Task<IResponse> HandleAsync(PublishNotificationToConnectedHubCommand request)
    {
        var channel = request.HubInstanceId;

        if (string.IsNullOrEmpty(channel))
        {
            Console.WriteLine($"Channel not found. So publish event to redis skipped\n");
            return Response.Success();
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

        return Response.Success();
    }
}