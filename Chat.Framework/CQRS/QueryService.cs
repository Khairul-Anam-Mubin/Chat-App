using Chat.Framework.Identity;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Results;

namespace Chat.Framework.CQRS;

public interface IQueryService : IQueryExecutor
{
    Task<TResponse> GetResponseAsync<TQuery, TResponse>(TQuery query)
    where TQuery : class, IInternalMessage
    where TResponse : class;
}

public class QueryService : IQueryService
{
    private readonly IQueryExecutor _queryExecutor;
    private readonly IMessageRequestClient _messageRequestClient;
    private readonly IScopeIdentity _scopeIdentity;

    public QueryService(IQueryExecutor queryExecutor, IMessageRequestClient messageRequestClient, IScopeIdentity scopeIdentity)
    {
        _queryExecutor = queryExecutor;
        _messageRequestClient = messageRequestClient;
        _scopeIdentity = scopeIdentity;
    }

    public async Task<IResult<TResponse>> ExecuteAsync<TQuery, TResponse>(TQuery query) 
        where TQuery : class, IQuery
        where TResponse : class
    {
        return await _queryExecutor.ExecuteAsync<TQuery, TResponse>(query);
    }

    public async Task<TResponse> GetResponseAsync<TQuery, TResponse>(TQuery query)
        where TQuery : class, IInternalMessage
        where  TResponse : class
    {
        query.Token = _scopeIdentity.GetToken();
        return await _messageRequestClient.GetResponseAsync<TQuery, TResponse>(query);
    }
}
