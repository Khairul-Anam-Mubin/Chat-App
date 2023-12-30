using System.ComponentModel.DataAnnotations;
using Chat.Framework.CQRS;

namespace Chat.Notification.Application.Commands;

public class SendNotificationToClientCommand : ICommand
{
    [Required]
    public Chat.Domain.Shared.Entities.Notification? Notification { get; set; }

    [Required]
    public List<string> ReceiverIds { get; set; }

    public SendNotificationToClientCommand()
    {
        ReceiverIds = new List<string>();
    }
}