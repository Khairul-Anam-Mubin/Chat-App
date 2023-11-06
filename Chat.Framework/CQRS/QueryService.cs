using Chat.Framework.Enums;
using Chat.Framework.Interfaces;
using Chat.Framework.Mediators;
using Chat.Framework.Models;

namespace Chat.Framework.CQRS;

public class QueryService : IQueryService
{
    private readonly IRequestMediator _requestMediator;

    public QueryService(IRequestMediator requestMediator)
    {
        _requestMediator = requestMediator;
    }

    public async Task<TResponse> GetResponseAsync<TQuery, TResponse>(TQuery query)
        where TQuery : class
        where TResponse : class, IResponse
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

            var response = new Response
            {
                Message = e.Message,
                Status = ResponseStatus.Error
            };

            return (response as TResponse)!;
        }
    }

    public async Task<IQueryResponse> GetResponseAsync<TQuery>(TQuery query) 
        where TQuery : class
    {
        return await GetResponseAsync<TQuery, QueryResponse>(query);
    }
}