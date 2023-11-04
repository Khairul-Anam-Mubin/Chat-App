using Chat.Framework.Mediators;
using Chat.Framework.MessageBrokers;

namespace Chat.Framework.CQRS;

public abstract class ACommandConsumer<TCommand, TResponse> : 
    AMessageConsumer<TCommand>,
    IRequestHandler<TCommand, TResponse>
    where TCommand : class
    where TResponse : class
{
    protected abstract Task<TResponse> OnConsumeAsync(TCommand command, IMessageContext<TCommand>? context = null);

    public override async Task Consume(IMessageContext<TCommand> context)
    {
        await OnConsumeAsync(context.Message, context);
    }

    public async Task<TResponse> HandleAsync(TCommand request)
    {
        return await OnConsumeAsync(request);
    }
}