using Chat.Domain.Models;
using Chat.Framework.CQRS;

namespace Chat.Application.Commands;

public class SendMessageCommand : ICommand
{
    public ChatModel? ChatModel { get; set; }
    public bool IsGroupMessage { get; set;}
}