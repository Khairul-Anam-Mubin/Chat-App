﻿using Chat.Framework.CQRS;
using Chat.Framework.PubSub;
using Chat.Framework.Results;
using Chat.Notification.Application.Commands;

namespace Chat.Notification.Application.CommandHandlers;

public class PublishNotificationToConnectedHubCommandHandler : ICommandHandler<PublishNotificationToConnectedHubCommand>
{
    private readonly IPubSub _pubSub;

    public PublishNotificationToConnectedHubCommandHandler(IPubSub pubSub)
    {
        _pubSub = pubSub;
    }

    public async Task<IResult> HandleAsync(PublishNotificationToConnectedHubCommand request)
    {
        var channel = request.HubId;

        var pubSubMessage = new PubSubMessage
        {
            Id = request.Notification.Id,
            Message = new SendNotificationToClientCommand(request.Notification, request.ReceiverUserIds),
            MessageType = MessageType.Notification
        };

        await _pubSub.PublishAsync(channel, pubSubMessage);

        Console.WriteLine("Event published to redis\n");

        return Result.Success();
    }
}