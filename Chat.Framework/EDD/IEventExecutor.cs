namespace Chat.Framework.EDD;

public interface IEventExecutor
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent;
}
