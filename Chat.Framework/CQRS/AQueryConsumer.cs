using Chat.Framework.Mediators;
using Chat.Framework.MessageBrokers;

namespace Chat.Framework.CQRS;

public abstract class AQueryConsumer<TQuery, TResponse> :
    AMessageConsumer<TQuery>,
    IHandler<TQuery, TResponse>
    where TQuery : class
    where TResponse : class
{
    protected abstract Task<TResponse> OnConsumeAsync(TQuery query, IMessageContext<TQuery>? context = null);

    public override async Task Consume(IMessageContext<TQuery> context)
    {
        var response = await OnConsumeAsync(context.Message, context);
        await context.RespondAsync(response);
    }

    public async Task<TResponse> HandleAsync(TQuery request)
    {
        return await OnConsumeAsync(request);
    }
}