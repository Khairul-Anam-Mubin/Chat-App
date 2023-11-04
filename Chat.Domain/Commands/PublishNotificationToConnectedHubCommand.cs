using Chat.Domain.Shared.Entities;
using Chat.Framework.CQRS;

namespace Chat.Domain.Commands;

public class PublishNotificationToConnectedHubCommand : ACommand
{
    public string HubInstanceId { get; set; } = string.Empty;
    public Notification? Notification { get; set; }
    public List<string> ReceiverIds { get; set; }

    public PublishNotificationToConnectedHubCommand()
    {
        ReceiverIds = new List<string>();
    }
}