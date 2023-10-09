using Chat.Framework.Mediators;

namespace Chat.Framework.CQRS;

public abstract class AQueryHandler<TQuery> : IRequestHandler<TQuery, QueryResponse> where TQuery : IQuery
{
    protected abstract Task<QueryResponse> OnHandleAsync(TQuery query);

    public async Task<QueryResponse> HandleAsync(TQuery query)
    {
        Console.WriteLine($"OnHandleAsync of : {GetType().Name}\n");
        try
        {
            var response = await OnHandleAsync(query);
            return query.CreateResponse(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            var response = query.CreateResponse();
            response.SetErrorMessage(e.Message);
            return response;
        }
    }
}