namespace Chat.Framework.DDD;

public interface IEntity
{
    public string Id { get; }

    public List<IDomainEvent> DomainEvents { get; }
}
