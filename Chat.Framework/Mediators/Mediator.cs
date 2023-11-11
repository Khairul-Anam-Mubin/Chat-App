using Chat.Framework.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Framework.Mediators;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
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

    protected virtual IHandler<TRequest, TResponse>? GetHandler<TRequest, TResponse>(string handlerName)
    {
        try
        {
            return _serviceProvider.GetRequiredService<IHandler<TRequest, TResponse>>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        var handler = _serviceProvider.GetService<IHandler>(handlerName);

        return (IHandler<TRequest, TResponse>?)handler; // todo: will use smart cast later
    }

    protected virtual IHandler<TRequest>? GetHandler<TRequest>(string handlerName)
    {
        try
        {
            return _serviceProvider.GetRequiredService<IHandler<TRequest>>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        var handler = _serviceProvider.GetService<IHandler>(handlerName);

        return (IHandler<TRequest>?)handler; // todo: will use smart cast later
    }
}