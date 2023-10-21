using Chat.Domain.Models;
using Chat.Framework.Events;

namespace Chat.Domain.Events;

public class PublishMessageToConnectedHubEvent : IEvent
{
    public string UserId { get; set; } = string.Empty;
    public string SendTo { get; set; } = string.Empty;
    public string MessageId { get; set; } = string.Empty;
    public ChatModel ChatModel { get; set; }
}