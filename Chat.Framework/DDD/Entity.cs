namespace Chat.Framework.DDD;

public abstract class Entity : IEntity
{
    public string Id { get; private set; }

    protected Entity(string id)
    {
        Id = id;
    }

    private List<IDomainEvent> _domainEvents = new();
    
    public List<IDomainEvent> DomainEvents 
    { 
        get 
        {
            _domainEvents = _domainEvents ?? new List<IDomainEvent>();
            return _domainEvents.ToList();
        } 
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        if (domainEvent is null) return;

        _domainEvents ??= new List<IDomainEvent>();

        _domainEvents.Add(domainEvent);
    }
}
