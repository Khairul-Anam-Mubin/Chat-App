using Chat.Framework.Identity;
using Chat.Framework.MessageBrokers;

namespace Chat.Framework.CQRS;

public abstract class AEventConsumer<TEvent> : AMessageConsumer<TEvent> 
    where TEvent : class, IEvent, IInternalMessage
{
    protected readonly IScopeIdentity ScopeIdentity;

    protected AEventConsumer(IScopeIdentity scopeIdentity)
    {
        ScopeIdentity = scopeIdentity;
    }

    protected abstract Task OnConsumeAsync(TEvent @event, IMessageContext<TEvent>? context = null);

    public override async Task Consume(IMessageContext<TEvent> context)
    {
        ScopeIdentity.SwitchIdentity(context.Message.Token);

        await OnConsumeAsync(context.Message, context);
    }
}
