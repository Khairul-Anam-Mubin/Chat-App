using System.ComponentModel.DataAnnotations;
using Chat.Domain.Shared.Entities;
using Chat.Framework.CQRS;

namespace Chat.Notification.Application.Commands;

public class PublishNotificationToConnectedHubCommand : ICommand
{
    [Required]
    public string HubInstanceId { get; set; } = string.Empty;

    [Required]
    public NotificationData? Notification { get; set; }

    [Required]
    public List<string> ReceiverIds { get; set; }

    public PublishNotificationToConnectedHubCommand()
    {
        ReceiverIds = new List<string>();
    }
}