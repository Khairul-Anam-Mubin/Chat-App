using Chat.Framework.CQRS;

namespace Chat.Notification.Application.Commands;

public class SendNotificationToClientCommand : ICommand
{
    public Chat.Domain.Shared.Entities.Notification? Notification { get; set; }
    public List<string> ReceiverIds { get; set; }

    public SendNotificationToClientCommand()
    {
        ReceiverIds = new List<string>();
    }
}