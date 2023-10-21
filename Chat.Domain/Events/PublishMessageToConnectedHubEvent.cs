using Chat.Domain.Models;

namespace Chat.Domain.Events;

public class PublishMessageToConnectedHubEvent
{
    public string UserId { get; set; } = string.Empty;
    public string SendTo { get; set; } = string.Empty;
    public string MessageId { get; set; } = string.Empty;
    public ChatModel ChatModel { get; set; }
}