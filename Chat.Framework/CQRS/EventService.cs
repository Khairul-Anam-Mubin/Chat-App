using Chat.Framework.Identity;
using Chat.Framework.MessageBrokers;

namespace Chat.Framework.CQRS;

public interface IEventService
{
    Task PublishAsync<TEvent>(TEvent @event) 
        where TEvent : class, IEvent, IInternalMessage;
}

public class EventService : IEventService
{
    private readonly IEventBus _eventBus;
    private readonly IScopeIdentity _scopeIdentity;

    public EventService(IEventBus eventBus, IScopeIdentity scopeIdentity)
    {
        _eventBus = eventBus;
        _scopeIdentity = scopeIdentity;
    }

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent, IInternalMessage
    {
        @event.Token = _scopeIdentity.GetToken();
        await _eventBus.PublishAsync(@event);
    }
}
