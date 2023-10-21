using Chat.Framework.Attributes;
using Chat.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Framework.Events;

[ServiceRegister(typeof(IEventPublisher), ServiceLifetime.Singleton)]
public class EventPublisher : IEventPublisher
{
    private readonly IRequestMediator _requestMediator;

    public EventPublisher(IRequestMediator requestMediator)
    {
        _requestMediator = requestMediator;
    }

    public Task PublishAsync<TEvent>(TEvent messageEvent) where TEvent : class, IEvent
    {
        return _requestMediator.SendAsync(messageEvent);
    }
}