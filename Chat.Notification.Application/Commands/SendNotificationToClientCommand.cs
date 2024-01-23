using System.ComponentModel.DataAnnotations;
using Chat.Domain.Shared.Entities;
using Chat.Framework.CQRS;

namespace Chat.Notification.Application.Commands;

public class SendNotificationToClientCommand : ICommand
{
    [Required]
    public NotificationData? Notification { get; set; }

    [Required]
    public List<string> ReceiverIds { get; set; }

    public SendNotificationToClientCommand()
    {
        ReceiverIds = new List<string>();
    }
}