using Chat.Framework.CQRS;

namespace Chat.Notification.Domain.Commands;

public class PublishNotificationToConnectedHubCommand
{
    public string HubInstanceId { get; set; } = string.Empty;
    public Chat.Domain.Shared.Entities.Notification? Notification { get; set; }
    public List<string> ReceiverIds { get; set; }

    public PublishNotificationToConnectedHubCommand()
    {
        ReceiverIds = new List<string>();
    }
}