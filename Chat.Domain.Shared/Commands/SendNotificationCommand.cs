﻿using System.ComponentModel.DataAnnotations;
using Chat.Domain.Shared.Entities;
using Peacious.Framework.CQRS;

namespace Chat.Domain.Shared.Commands;

public class SendNotificationCommand : ICommand, IInternalMessage
{
    [Required]
    public NotificationData Notification { get; set; }

    [Required]
    public List<string> ReceiverUserIds { get; set; }
    public string? Token { get; set; }

    public SendNotificationCommand(NotificationData notification, List<string> receiverUserIds)
    {
        Notification = notification;
        ReceiverUserIds = receiverUserIds;
    }
}