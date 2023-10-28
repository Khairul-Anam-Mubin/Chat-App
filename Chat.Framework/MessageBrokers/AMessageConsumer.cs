using MassTransit;

namespace Chat.Framework.MessageBrokers;

public abstract class AMessageConsumer<TMessage> : IMessageConsumer<TMessage>, IConsumer<TMessage> where TMessage : class
{
    public abstract Task Consume(IMessageContext<TMessage> context);

    public async Task Consume(ConsumeContext<TMessage> context)
    {
        await Consume(new MassTransitConsumeContext<TMessage>(context));
    }
}