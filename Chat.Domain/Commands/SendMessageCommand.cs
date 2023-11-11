using Chat.Domain.Models;

namespace Chat.Domain.Commands;

public class SendMessageCommand
{
    public ChatModel? ChatModel {get; set;}
}