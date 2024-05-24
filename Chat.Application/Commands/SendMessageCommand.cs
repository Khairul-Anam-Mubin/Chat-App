using Peacious.Framework.CQRS;

namespace Chat.Application.Commands;

public class SendMessageCommand : ICommand
{
    public string ReceiverId { get; set; }
    public string MessageContent { get; set; }
    public bool IsGroupMessage { get; set;}
}