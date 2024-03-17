using Chat.Domain.Entities;
using Chat.Framework.DDD;

namespace Chat.Domain.DomainEvents;

public class ChatCreatedDomainEvent : DomainEvent
{
    public ChatModel ChatModel { get; set; }

    public ChatCreatedDomainEvent(ChatModel chatModel) : base(chatModel.Id)
    {
        ChatModel = chatModel;
    }
}
