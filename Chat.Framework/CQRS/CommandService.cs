using Chat.Framework.Attributes;
using Chat.Framework.Enums;
using Chat.Framework.Interfaces;
using Chat.Framework.Mediators;
using Chat.Framework.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Framework.CQRS;

[ServiceRegister(typeof(ICommandService), ServiceLifetime.Transient)]
public class CommandService : ICommandService
{
    private readonly IRequestMediator _requestMediator;

    public CommandService(IRequestMediator requestMediator)
    {
        _requestMediator = requestMediator;
    }

    public async Task<TResponse> GetResponseAsync<TCommand, TResponse>(TCommand command)
        where TCommand : class
        where TResponse : class, IResponse
    {
        try
        {
            var response = await _requestMediator.SendAsync<TCommand, TResponse>(command);

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

    public async Task<IResponse> GetResponseAsync<TCommand>(TCommand command) 
        where TCommand : class
    {
        return await GetResponseAsync<TCommand, Response>(command);
    }
}