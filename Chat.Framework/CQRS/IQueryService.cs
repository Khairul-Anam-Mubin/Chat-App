using Chat.Framework.Interfaces;

namespace Chat.Framework.CQRS;

public interface IQueryService
{
    Task<TResponse> GetResponseAsync<TQuery, TResponse>(TQuery query)
        where TQuery : class, IQuery 
        where TResponse : class, IResponse;

    Task<IQueryResponse> GetResponseAsync<TQuery>(TQuery query)
        where TQuery : class, IQuery;
}