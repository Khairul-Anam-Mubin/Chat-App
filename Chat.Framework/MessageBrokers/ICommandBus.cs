namespace Chat.Framework.MessageBrokers;

public interface ICommandBus
{
    Task SendAsync<TCommand> (TCommand command) where TCommand : class;
}