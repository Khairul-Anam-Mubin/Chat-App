using System.ComponentModel.DataAnnotations;
using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Entities;

namespace Chat.Notification.Application.Commands;

public class PublishNotificationToConnectedHubCommand : SendNotificationCommand
{
    [Required]
    public string HubId { get; set; }

    public PublishNotificationToConnectedHubCommand(string hubId, NotificationData notification, List<string> receiverUserIds) 
        : base(notification, receiverUserIds)
    {
        HubId = hubId;
    }
}