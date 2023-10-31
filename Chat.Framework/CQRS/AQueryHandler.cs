using Chat.Framework.Enums;
using Chat.Framework.Interfaces;
using Chat.Framework.Mediators;

namespace Chat.Framework.CQRS;

public abstract class AQueryHandler<TQuery, TResponse> : 
    IRequestHandler<TQuery, TResponse> 
    where TQuery : class, IQuery
    where TResponse : class, IResponse
{
    protected abstract Task<TResponse> OnHandleAsync(TQuery query);

    public async Task<TResponse> HandleAsync(TQuery query)
    {
        Console.WriteLine($"OnHandleAsync of : {GetType().Name}\n");
        try
        {
            var response = await OnHandleAsync(query);
            response.Status = ResponseStatus.Success;
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            var response = query.CreateResponse();
            response.Status = ResponseStatus.Error;
            return response as TResponse;
        }
    }
}