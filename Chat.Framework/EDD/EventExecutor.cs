using MediatR;

namespace Chat.Framework.EDD;

public class EventExecutor : IEventExecutor
{
    private readonly IMediator _mediator;

    public EventExecutor(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task PublishAsync<TEvent>(TEvent @event)
        where TEvent : class, IEvent
    {
        await _mediator.Publish(@event);
    }
}
