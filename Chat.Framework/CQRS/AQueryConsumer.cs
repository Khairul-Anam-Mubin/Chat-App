using Chat.Framework.Mediators;
using Chat.Framework.MessageBrokers;

namespace Chat.Framework.CQRS;

public abstract class AQueryConsumer<TQuery> :
    AMessageConsumer<TQuery>,
    IRequestHandler<TQuery, QueryResponse> 
    where TQuery : class, IQuery
{
    protected abstract Task<QueryResponse> OnConsumeAsync(TQuery query, IMessageContext<TQuery>? context = null);

    public override async Task Consume(IMessageContext<TQuery> context)
    {
        await OnConsumeAsync(context.Message, context);
    }

    public async Task<QueryResponse> HandleAsync(TQuery request)
    {
        return await OnConsumeAsync(request);
    }
}