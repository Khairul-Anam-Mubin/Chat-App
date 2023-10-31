namespace Chat.Framework.CQRS;

public interface IQueryService
{
    Task<TResponse> GetResponseAsync<TQuery, TResponse>(TQuery query)
        where TQuery : class, IQuery 
        where TResponse : class, IQueryResponse;

    Task<IQueryResponse> GetResponseAsync<TQuery>(TQuery query)
        where TQuery : class, IQuery;
}