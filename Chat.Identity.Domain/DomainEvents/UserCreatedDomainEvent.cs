using Chat.Framework.DDD;
using Chat.Identity.Domain.Entities;

namespace Chat.Identity.Domain.DomainEvents;

public class UserCreatedDomainEvent : DomainEvent
{
    public User User { get; set; }

    public UserCreatedDomainEvent(User user) : base(user.Id)
    {
        User = user;
    }
}
