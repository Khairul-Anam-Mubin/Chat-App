namespace Chat.Framework.Events;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent messageEvent) where TEvent : class, IEvent;
}