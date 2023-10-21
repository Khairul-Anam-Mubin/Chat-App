using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Enums;
using Chat.Framework.Extensions;
using Chat.Framework.Mediators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Framework.Proxy;

[ServiceRegister(typeof(ICommandQueryProxy), ServiceLifetime.Singleton)]
public class CommandQueryProxy : ICommandQueryProxy
{
    private readonly IRequestMediator _requestMediator;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHttpClientFactory _httpClientFactory;

    public CommandQueryProxy(
        IRequestMediator requestMediator, 
        IConfiguration configuration, 
        IHttpContextAccessor httpContextAccessor, 
        IHttpClientFactory httpClientFactory) 
    {
        _requestMediator = requestMediator;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<CommandResponse> GetCommandResponseAsync<TCommand>(TCommand command) where TCommand : ICommand
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

    public async Task<QueryResponse> GetQueryResponseAsync<TQuery>(TQuery query) where TQuery : IQuery
    {
        QueryResponse response;
        try
        {
            response = await _requestMediator.SendAsync<TQuery, QueryResponse>(query);
            response = query.CreateResponse(response);
            response.Status = ResponseStatus.Success;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            response = query.CreateResponse();
            response.Status = ResponseStatus.Error;
            response.Message = e.Message;
        }
        return response;
    }

    private bool IsCurrentApi<TCommand>(TCommand command) where TCommand : ICommand
    {
        if (string.IsNullOrEmpty(command.ApiUrl)) return true;

        var currentApiOrigin = _configuration.GetSection("ApiOrigin").Value;

        return command.ApiUrl.StartsWith(currentApiOrigin);
    }
}