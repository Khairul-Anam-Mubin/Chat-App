using Chat.Framework.Mediators;

namespace Chat.Framework.CQRS;

public class QueryClient : IQueryClient
{
    private readonly IRequestMediator _requestMediator;

    public QueryClient(IRequestMediator requestMediator)
    {
        _requestMediator = requestMediator;
    }

    public async Task<TResponse> GetResponseAsync<TQuery, TResponse>(TQuery query) 
        where TQuery : class, IQuery
        where TResponse : QueryResponse
    {
        return await _requestMediator.SendAsync<TQuery, TResponse>(query);
    }
}