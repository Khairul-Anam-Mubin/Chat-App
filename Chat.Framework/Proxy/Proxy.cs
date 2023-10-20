using Chat.Framework.Attributes;
using Chat.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Framework.Proxy;

[ServiceRegister(typeof(IProxy), ServiceLifetime.Singleton)]
public class Proxy : IProxy
{
    private readonly IRequestMediator _requestMediator;

    public Proxy(IRequestMediator mediator)
    {
        _requestMediator = mediator;
    }
        
    public async Task SendAsync<TRequest>(TRequest request)
    {
        await Task.Factory.StartNew(
            () => _requestMediator.HandleAsync<TRequest>(request));
    }
}