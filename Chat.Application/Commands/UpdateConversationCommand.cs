using Chat.Domain.Entities;
using Chat.Framework.CQRS;

namespace Chat.Application.Commands;

public class UpdateConversationCommand : ICommand
{
    public Conversation? Conversation { get; set; }
}