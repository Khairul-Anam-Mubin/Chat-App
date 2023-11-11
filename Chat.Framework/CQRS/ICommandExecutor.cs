using Chat.Framework.RequestResponse;

namespace Chat.Framework.CQRS;

public interface ICommandExecutor
{
    Task<TResponse> ExecuteAsync<TCommand, TResponse>(TCommand command)
        where TCommand : class
        where TResponse : class, IResponse;

    Task<IResponse> ExecuteAsync<TCommand>(TCommand command)
        where TCommand : class;
}