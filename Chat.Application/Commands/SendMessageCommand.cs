using Chat.Domain.Models;

namespace Chat.Application.Commands;

public class SendMessageCommand
{
    public ChatModel? ChatModel { get; set; }
}