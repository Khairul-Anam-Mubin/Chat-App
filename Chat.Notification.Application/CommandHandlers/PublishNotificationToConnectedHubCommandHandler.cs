using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.PubSub;
using Chat.Framework.Results;
using Chat.Notification.Application.Commands;

namespace Chat.Notification.Application.CommandHandlers;

public class PublishNotificationToConnectedHubCommandHandler : ICommandHandler<PublishNotificationToConnectedHubCommand>
{
    private readonly IPubSub _pubSub;
    private readonly IScopeIdentity _scopeIdentity;

    public PublishNotificationToConnectedHubCommandHandler(IPubSub pubSub, IScopeIdentity scopeIdentity)
    {
        _scopeIdentity = scopeIdentity;
        _pubSub = pubSub;
    }

    public async Task<IResult> HandleAsync(PublishNotificationToConnectedHubCommand request)
    {
        var channel = request.HubId;

        var pubSubMessage = new PubSubMessage
        {
            Id = request.Notification.Id,
            Message = new SendNotificationToClientCommand(request.Notification, request.ReceiverUserIds),
            MessageType = MessageType.Notification,
            Token = _scopeIdentity.GetToken()
        };

        await _pubSub.PublishAsync(channel, pubSubMessage);

        Console.WriteLine("Event published to redis\n");

        return Result.Success();
    }
}