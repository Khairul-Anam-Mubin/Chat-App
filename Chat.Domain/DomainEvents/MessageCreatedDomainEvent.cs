using Chat.Domain.Entities;
using Chat.Framework.DDD;

namespace Chat.Domain.DomainEvents;

public class MessageCreatedDomainEvent : DomainEvent
{
    public Message Message { get; set; }

    public MessageCreatedDomainEvent(Message message) : base(message.Id)
    {
        Message = message;
    }
}
