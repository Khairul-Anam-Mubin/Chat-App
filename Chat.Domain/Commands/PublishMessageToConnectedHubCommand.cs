using Chat.Domain.Models;
using Chat.Framework.CQRS;

namespace Chat.Domain.Commands;

public class PublishMessageToConnectedHubCommand : ACommand
{
    public string UserId { get; set; } = string.Empty;
    public string SendTo { get; set; } = string.Empty;
    public string MessageId { get; set; } = string.Empty;
    public ChatModel ChatModel { get; set; }
}