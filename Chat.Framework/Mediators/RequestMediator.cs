using Chat.Framework.Attributes;
using Chat.Framework.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Framework.Mediators;

[ServiceRegister(typeof(IRequestMediator), ServiceLifetime.Singleton)]
public class RequestMediator : IRequestMediator
{
    private readonly IServiceProvider _serviceProvider;

    public RequestMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request)
    {
        var handlerName = GetHandlerName(request);

        var handler = GetHandler<TRequest, TResponse>(handlerName) ?? 
                      throw new Exception($"{handlerName} not found");

        return await handler.HandleAsync(request);
    }
    
    public async Task SendAsync<TRequest>(TRequest request)
    {
        var handlerName = GetHandlerName(request);

        var handler = GetHandler<TRequest>(handlerName) ??
                      throw new Exception("Handler not found");

        await handler.HandleAsync(request);
    }

    protected virtual string GetHandlerNameSuffix()
    {
        return "Handler";
    }

    protected virtual string GetHandlerName<TRequest>(TRequest request)
    {
        return request?.GetType().Name + GetHandlerNameSuffix();
    }

    protected virtual IRequestHandler<TRequest, TResponse>? GetHandler<TRequest, TResponse>(string handlerName)
    {
        try
        {
            return _serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        var handler = _serviceProvider.GetService<IRequestHandler>(handlerName);

        return (IRequestHandler<TRequest, TResponse>?)handler; // todo: will use smart cast later
    }

    protected virtual IRequestHandler<TRequest>? GetHandler<TRequest>(string handlerName)
    {
        try
        {
            return _serviceProvider.GetRequiredService<IRequestHandler<TRequest>>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        var handler = _serviceProvider.GetService<IRequestHandler>(handlerName);

        return (IRequestHandler<TRequest>?)handler; // todo: will use smart cast later
    }
}