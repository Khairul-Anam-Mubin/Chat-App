using Chat.Framework.Identity;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Results;

namespace Chat.Framework.CQRS;

public abstract class AQueryConsumer<TQuery, TResponse> :
    AMessageConsumer<TQuery>,
    IQueryHandler<TQuery, TResponse>
    where TQuery : class, IQuery, IInternalMessage
    where TResponse : class
{
    protected readonly IScopeIdentity ScopeIdentity;

    protected AQueryConsumer(IScopeIdentity scopeIdentity)
    {
        ScopeIdentity = scopeIdentity;
    }

    protected abstract Task<IResult<TResponse>> OnConsumeAsync(TQuery query, IMessageContext<TQuery>? context = null);

    public override async Task Consume(IMessageContext<TQuery> context)
    {
        ScopeIdentity.SwitchIdentity(context.Message.Token);
        var response = await OnConsumeAsync(context.Message, context);
        await context.RespondAsync(response.Value!);
    }

    public async Task<IResult<TResponse>> HandleAsync(TQuery request)
    {
        return await OnConsumeAsync(request);
    }
}