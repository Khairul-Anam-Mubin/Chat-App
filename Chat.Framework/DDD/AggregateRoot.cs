namespace Chat.Framework.DDD;

public abstract class AggregateRoot : AEntity
{
    protected AggregateRoot(string id) : base(id) {}
}
