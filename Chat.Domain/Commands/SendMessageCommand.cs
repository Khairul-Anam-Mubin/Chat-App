using Chat.Domain.Models;
using Chat.Framework.CQRS;

namespace Chat.Domain.Commands;

public class SendMessageCommand
{
    public ChatModel? ChatModel {get; set;}
}