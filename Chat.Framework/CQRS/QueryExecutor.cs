using Chat.Framework.Mediators;
using Chat.Framework.Results;

namespace Chat.Framework.CQRS;

public class QueryExecutor : IQueryExecutor
{
    private readonly IMediator _mediator;

    public QueryExecutor(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IResult<TResponse>> ExecuteAsync<TQuery, TResponse>(TQuery query) 
        where TQuery : class, IQuery
        where TResponse : class
    {
        try
        {
            var response = await _mediator.SendAsync<TQuery, IResult<TResponse>>(query);

            response.Status ??= ResponseStatus.Success;

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

            return Result.Error<TResponse>(e.Message);
        }
    }
}