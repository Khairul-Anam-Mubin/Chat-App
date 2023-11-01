using Chat.Framework.Interfaces;
using Chat.Framework.Mediators;
using Chat.Framework.MessageBrokers;

namespace Chat.Framework.CQRS;

public abstract class AQueryConsumer<TQuery, TResponse> :
    AMessageConsumer<TQuery>,
    IRequestHandler<TQuery, TResponse>
    where TResponse : class, IResponse
    where TQuery : class, IQuery
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