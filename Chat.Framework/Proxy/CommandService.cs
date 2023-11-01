using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Enums;
using Chat.Framework.Extensions;
using Chat.Framework.Interfaces;
using Chat.Framework.Mediators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Framework.Proxy;

[ServiceRegister(typeof(ICommandService), ServiceLifetime.Transient)]
public class CommandService : ICommandService
{
    private readonly IRequestMediator _requestMediator;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CommandService(
        IRequestMediator requestMediator, 
        IConfiguration configuration, 
        IHttpContextAccessor httpContextAccessor, 
        IHttpClientFactory httpClientFactory, 
        IServiceScopeFactory serviceScopeFactory) 
    {
        _requestMediator = requestMediator;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _httpClientFactory = httpClientFactory;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<IResponse> GetResponseAsync<TCommand>(TCommand command) 
        where TCommand : class, ICommand
    {
        return await GetResponseAsync<TCommand, CommandResponse>(command);
    }

    public async Task<TResponse> GetResponseAsync<TCommand, TResponse>(TCommand command) 
        where TCommand : class, ICommand
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

            var response = command.CreateResponse() as TResponse;
            response.Message = e.Message;
            response.Status = ResponseStatus.Error;

            return response;
        }
    }
}