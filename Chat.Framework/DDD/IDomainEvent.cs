using Chat.Framework.EDD;

namespace Chat.Framework.DDD;

public interface IDomainEvent : IEvent
{
    public string Id { get; }
}