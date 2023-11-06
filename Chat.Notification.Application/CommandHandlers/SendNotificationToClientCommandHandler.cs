﻿using Chat.Framework.Attributes;
using Chat.Framework.Mediators;
using Chat.Framework.Models;
using Chat.Notification.Domain.Commands;
using Chat.Notification.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Notification.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<SendNotificationToClientCommand, Response>), ServiceLifetime.Transient)]
public class SendNotificationToClientCommandHandler :
    IRequestHandler<SendNotificationToClientCommand, Response>
{
    private readonly INotificationHubService _notificationHubService;

    public SendNotificationToClientCommandHandler(INotificationHubService notificationHubService)
    {
        _notificationHubService = notificationHubService;
    }

    public async Task<Response> HandleAsync(SendNotificationToClientCommand request)
    {
        var notification = request.Notification;
        var receiverIds = request.ReceiverIds;

        foreach (var receiverId in receiverIds)
        {
            await _notificationHubService.SendAsync(receiverId, notification, notification!.NotificationType.ToString());
        }

        return Response.Create();
    }
}