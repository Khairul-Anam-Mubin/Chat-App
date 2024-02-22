using Chat.Framework.Extensions;
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
        var validationResult = query.GetValidationResult<TResponse>();

        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        try
        {
            var response = await _mediator.SendAsync<TQuery, IResult<TResponse>>(query);

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

            return Result.Error<TResponse>(e.Message);
        }
    }
}