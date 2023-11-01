using Chat.Framework.Interfaces;

namespace Chat.Framework.CQRS;

public interface ICommandService
{
    Task<TResponse> GetResponseAsync<TCommand, TResponse>(TCommand command)
        where TCommand : class, ICommand
        where TResponse : class, IResponse;

    Task<IResponse> GetResponseAsync<TCommand>(TCommand command) 
        where TCommand : class, ICommand;
}