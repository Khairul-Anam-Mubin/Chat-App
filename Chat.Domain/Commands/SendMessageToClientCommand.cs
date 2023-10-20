using Chat.Domain.Models;
using Chat.Framework.CQRS;

namespace Chat.Domain.Commands;

public class SendMessageToClientCommand : ACommand
{
    public string MessageId { get; set; } = string.Empty;
    public ChatModel? ChatModel { get; set; }
}