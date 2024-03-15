namespace Chat.Framework.DDD;

public abstract class AEntity : IEntity
{
    public string Id { get; private set; }

    protected AEntity(string id)
    {
        Id = id;
        _domainEvents = new();
    }

    private AEntity()
    {
        Id = Guid.NewGuid().ToString();
        _domainEvents = new();
    }

    private List<IDomainEvent> _domainEvents;
    public List<IDomainEvent> DomainEvents => _domainEvents is null ? new() : _domainEvents.ToList();

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        if (domainEvent is null) return;

        _domainEvents ??= new List<IDomainEvent>();

        _domainEvents.Add(domainEvent);
    }
}
