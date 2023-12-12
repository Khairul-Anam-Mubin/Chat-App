using Chat.Framework.MessageBrokers;
using Chat.Framework.Results;

namespace Chat.Framework.CQRS;

public abstract class AQueryConsumer<TQuery, TResponse> :
    AMessageConsumer<TQuery>,
    IQueryHandler<TQuery, TResponse>
    where TQuery : class, IQuery
    where TResponse : class
{
    protected abstract Task<IResult<TResponse>> OnConsumeAsync(TQuery query, IMessageContext<TQuery>? context = null);

    public override async Task Consume(IMessageContext<TQuery> context)
    {
        var response = await OnConsumeAsync(context.Message, context);
        await context.RespondAsync(response);
    }

    public async Task<IResult<TResponse>> HandleAsync(TQuery request)
    {
        return await OnConsumeAsync(request);
    }
}