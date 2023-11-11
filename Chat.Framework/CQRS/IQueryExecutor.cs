using Chat.Framework.RequestResponse;

namespace Chat.Framework.CQRS;

public interface IQueryExecutor
{
    Task<TResponse> ExecuteAsync<TQuery, TResponse>(TQuery query)
        where TQuery : class
        where TResponse : class, IResponse;

    Task<IPaginationResponse> ExecuteAsync<TQuery>(TQuery query)
        where TQuery : class;
}