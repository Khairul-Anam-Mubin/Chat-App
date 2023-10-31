using Chat.Framework.Enums;
using Chat.Framework.Mediators;

namespace Chat.Framework.CQRS;

public class QueryService : IQueryService
{
    private readonly IRequestMediator _requestMediator;

    public QueryService(IRequestMediator requestMediator)
    {
        _requestMediator = requestMediator;
    }

    public async Task<TResponse> GetResponseAsync<TQuery, TResponse>(TQuery query) 
        where TQuery : class, IQuery
        where TResponse : class, IQueryResponse
    {
        try
        {
            var response = await _requestMediator.SendAsync<TQuery, TResponse>(query);
            response.Status = ResponseStatus.Success;
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

            var response = query.CreateResponse() as TResponse;
            response.Message = e.Message;
            response.Status = ResponseStatus.Error;
            
            return response;
        }
    }

    public async Task<IQueryResponse> GetResponseAsync<TQuery>(TQuery query) 
        where TQuery : class, IQuery
    {
        return await GetResponseAsync<TQuery, IQueryResponse>(query);
    }
}