namespace Chat.Framework.CQRS;

public interface IQueryService
{
    Task<TResponse> GetResponseAsync<TQuery, TResponse>(TQuery query)
        where TQuery : class, IQuery 
        where TResponse : class, IQueryResponse;
}