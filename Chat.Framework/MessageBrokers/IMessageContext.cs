namespace Chat.Framework.MessageBrokers;

public interface IMessageContext<out TMessage> : 
    IEventBus,
    ICommandBus
    where TMessage : class
{
    TMessage Message { get; }

    Task RespondAsync<T>(T message) where T : class;
}