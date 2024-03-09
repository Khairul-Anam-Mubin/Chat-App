using Chat.Domain.Entities;
using Chat.Framework.CQRS;

namespace Chat.Application.Commands;

public class SendMessageCommand : ICommand
{
    public ChatModel? ChatModel { get; set; }
    public string SendTo { get; set; }
    public string Message { get; set; }
    public bool IsGroupMessage { get; set;}
}