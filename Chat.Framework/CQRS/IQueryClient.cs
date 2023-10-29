namespace Chat.Framework.CQRS;

public interface IQueryClient
{
    Task<TResponse> GetResponseAsync<TQuery, TResponse>(TQuery query)
        where TQuery : class, IQuery 
        where TResponse : QueryResponse;
}