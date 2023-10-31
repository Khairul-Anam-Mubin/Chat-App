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

    public async Task<CommandResponse> GetResponseAsync<TCommand>(TCommand command) 
        where TCommand : class, ICommand
    {
        CommandResponse response;
        try
        {
            if (IsCurrentApi(command))
            {
                if (command.FireAndForget)
                {
                    _ = Task.Factory.StartNew(() => _requestMediator.SendAsync<TCommand, CommandResponse>(command));
                    response = command.CreateResponse();
                    response.Status = ResponseStatus.Pending;
                }
                else
                {
                    response = await _requestMediator.SendAsync<TCommand, CommandResponse>(command);
                    response = command.CreateResponse(response);
                    response.Status = ResponseStatus.Success;
                }
            }
            else
            {
                var accessToken = _httpContextAccessor.HttpContext?.GetAccessToken();

                response = await _httpClientFactory
                    .CreateClient()
                    .AddBearerToken(accessToken)
                    .PostAsync<CommandResponse>(command.ApiUrl, command);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            response = command.CreateResponse();
            response.SetErrorMessage(e.Message);
        }
        return response!;
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

    private bool IsCurrentApi<TCommand>(TCommand command) where TCommand : ICommand
    {
        if (string.IsNullOrEmpty(command.ApiUrl)) return true;

        var currentApiOrigin = _configuration.GetSection("ApiOrigin").Value;

        return command.ApiUrl.StartsWith(currentApiOrigin);
    }
}