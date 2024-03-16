using Chat.Domain.Entities;
using Chat.Framework.DDD;

namespace Chat.Domain.DomainEvents;

public class ChatCreatedDomainEvent : IDomainEvent
{
    public string Id { get; set; }
    
    public ChatModel ChatModel { get; set; }

    public ChatCreatedDomainEvent(ChatModel chatModel)
    {
        ChatModel = chatModel;
        Id = chatModel.Id;
    }
}
