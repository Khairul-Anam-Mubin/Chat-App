using Chat.Domain.Models;
using Chat.Framework.CQRS;

namespace Chat.Domain.Commands;

public class SendMessageCommand : ACommand
{
    public ChatModel? ChatModel {get; set;}
        
    public override void ValidateCommand()
    {
        if (ChatModel == null) 
        {
            throw new Exception("ChatModel not set");
        }
    }
}