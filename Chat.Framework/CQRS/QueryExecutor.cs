using Chat.Framework.Enums;
using Chat.Framework.Mediators;
using Chat.Framework.RequestResponse;

namespace Chat.Framework.CQRS;

public class QueryExecutor : IQueryExecutor
{
    private readonly IMediator _mediator;

    public QueryExecutor(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<TResponse> ExecuteAsync<TQuery, TResponse>(TQuery query)
        where TQuery : class
        where TResponse : class, IResponse
    {
        try
        {
            var response = await _mediator.SendAsync<TQuery, TResponse>(query);
            
            response.Status ??= ResponseStatus.Success;

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

            var response = Response.Error(e);

            return (response as TResponse)!;
        }
    }

    public async Task<IPaginationResponse> ExecuteAsync<TQuery>(TQuery query) 
        where TQuery : class
    {
        return await ExecuteAsync<TQuery, IPaginationResponse>(query);
    }
}