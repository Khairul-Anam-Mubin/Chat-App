using Chat.Domain.Entities;
using Chat.Framework.DDD;

namespace Chat.Domain.DomainEvents;

public class MessageCreatedDomainEvent : DomainEvent
{
    public string MessageId { get; set; }

    public MessageCreatedDomainEvent(string messageId) : base(messageId)
    {
        MessageId = messageId;
    }
}
