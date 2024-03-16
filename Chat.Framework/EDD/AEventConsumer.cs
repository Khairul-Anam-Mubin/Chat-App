using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.MessageBrokers;

namespace Chat.Framework.EDD;

public abstract class AEventConsumer<TEvent> : AMessageConsumer<TEvent>, IEventHandler<TEvent>
    where TEvent : class, IEvent, IInternalMessage
{
    protected readonly IScopeIdentity ScopeIdentity;

    protected abstract Task OnConsumeAsync(TEvent @event, IMessageContext<TEvent>? context = null);
    
    protected AEventConsumer(IScopeIdentity scopeIdentity)
    {
        ScopeIdentity = scopeIdentity;
    }

    public override async Task Consume(IMessageContext<TEvent> context)
    {
        ScopeIdentity.SwitchIdentity(context.Message.Token);

        await OnConsumeAsync(context.Message, context);
    }

    public async Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
        await OnConsumeAsync(notification);
    }
}
