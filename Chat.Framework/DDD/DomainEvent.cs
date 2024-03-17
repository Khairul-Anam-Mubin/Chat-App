namespace Chat.Framework.DDD;

public abstract class DomainEvent : IDomainEvent
{
    public string Id { get; private set; }

    protected DomainEvent(string id)
    {
        Id = id;
    }
}
