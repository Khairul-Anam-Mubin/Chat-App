using Chat.Framework.EDD;

namespace Chat.Framework.DDD;

public interface IDomainEventHandler<TDomainEvent> : IEventHandler<TDomainEvent>
    where TDomainEvent : class, IDomainEvent {}
