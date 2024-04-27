using KCluster.Framework.DDD;

namespace Chat.Identity.Domain.DomainEvents;

public class UserCreatedDomainEvent : DomainEvent
{
    public UserCreatedDomainEvent(string userId) : base(userId)
    {}
}
