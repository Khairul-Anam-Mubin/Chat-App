using Chat.Domain.Commands;
using Chat.Framework.Attributes;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Extensions;
using Chat.Framework.Mediators;
using Chat.Framework.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<PublishNotificationToConnectedHubCommand, Response>), ServiceLifetime.Transient)]
public class PublishNotificationToConnectedHubCommandHandler : 
    IRequestHandler<PublishNotificationToConnectedHubCommand, Response>
{
    private readonly IConfiguration _configuration;
    private readonly IRedisContext _redisContext;

    public PublishNotificationToConnectedHubCommandHandler(
        IConfiguration configuration, 
        IRedisContext redisContext)
    {
        _configuration = configuration;
        _redisContext = redisContext;
    }

    public async Task<Response> HandleAsync(PublishNotificationToConnectedHubCommand request)
    {
        var subscriber = _redisContext.GetSubscriber(
            _configuration.GetSection("RedisConfig:DatabaseInfo")
                .Get<DatabaseInfo>());

        var channel = request.HubInstanceId;

        if (string.IsNullOrEmpty(channel))
        {
            Console.WriteLine($"Channel not found. So publish event to redis skipped\n");
            return Response.Create();
        }

        await subscriber.PublishAsync(channel, new PubSubMessage
            {
                Id = request.Notification!.Id,
                Message = new SendNotificationToClientCommand
                {
                    Notification = request.Notification,
                    ReceiverIds = request.ReceiverIds,
                },
                MessageType = MessageType.Notification
            }.Serialize()
        );

        Console.WriteLine("Event published to redis\n");

        return Response.Create();
    }
}