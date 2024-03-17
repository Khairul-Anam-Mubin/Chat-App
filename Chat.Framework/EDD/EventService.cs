using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.MessageBrokers;

namespace Chat.Framework.EDD;

public class EventService : IEventService
{
    private readonly IEventBus _eventBus;
    private readonly IScopeIdentity _scopeIdentity;
    private readonly IEventExecutor _eventExecutor;

    public EventService(
        IEventBus eventBus,
        IScopeIdentity scopeIdentity,
        IEventExecutor eventExecutor)
    {
        _eventBus = eventBus;
        _scopeIdentity = scopeIdentity;
        _eventExecutor = eventExecutor;
    }

    public async Task PublishIntegrationEventAsync<TEvent>(TEvent @event)
        where TEvent : class, IEvent, IInternalMessage
    {
        @event.Token = _scopeIdentity.GetToken();
        await _eventBus.PublishAsync(@event);
    }

    public async Task PublishEventAsync<TEvent>(TEvent @event)
        where TEvent : class, IEvent
    {
        await _eventExecutor.PublishAsync(@event);
    }
}
