using Chat.Framework.MessageBrokers;
using Chat.Framework.Results;

namespace Chat.Framework.CQRS;

public abstract class ACommandConsumer<TCommand, TResponse> : 
    AMessageConsumer<TCommand>,
    ICommandHandler<TCommand, TResponse>
    where TCommand : class, ICommand
    where TResponse : class
{
    protected abstract Task<IResult<TResponse>> OnConsumeAsync(TCommand command, IMessageContext<TCommand>? context = null);

    public override async Task Consume(IMessageContext<TCommand> context)
    {
        await OnConsumeAsync(context.Message, context);
    }

    public async Task<IResult<TResponse>> HandleAsync(TCommand request)
    {
        return await OnConsumeAsync(request);
    }
}

public abstract class ACommandConsumer<TCommand> :
    AMessageConsumer<TCommand>,
    ICommandHandler<TCommand>
    where TCommand : class, ICommand
{
    protected abstract Task<IResult> OnConsumeAsync(TCommand command, IMessageContext<TCommand>? context = null);

    public override async Task Consume(IMessageContext<TCommand> context)
    {
        await OnConsumeAsync(context.Message, context);
    }

    public async Task<IResult> HandleAsync(TCommand request)
    {
        return await OnConsumeAsync(request);
    }
}