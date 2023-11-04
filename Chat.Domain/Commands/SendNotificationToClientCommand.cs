using Chat.Domain.Shared.Entities;
using Chat.Framework.CQRS;

namespace Chat.Domain.Commands;

public class SendNotificationToClientCommand : ACommand
{
    public Notification? Notification { get; set; }
    public List<string> ReceiverIds { get; set; }

    public SendNotificationToClientCommand()
    {
        ReceiverIds = new List<string>();
    }
}