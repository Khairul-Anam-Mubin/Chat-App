using Chat.Framework.Identity;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Results;

namespace Chat.Framework.CQRS;

public interface ICommandService : ICommandExecutor
{
    /// <summary>
    /// Sends the command to message queue
    /// </summary>
    Task SendAsync<TCommand>(TCommand command) where TCommand : class, IInternalMessage;
}

public class CommandService : ICommandService
{
    private readonly ICommandExecutor _commandExecutor;
    private readonly ICommandBus _commandBus;
    private readonly IScopeIdentity _scopeIdentity;

    public CommandService(ICommandExecutor commandExecutor, ICommandBus commandBus, IScopeIdentity scopeIdentity)
    {
        _commandExecutor = commandExecutor;
        _commandBus = commandBus;
        _scopeIdentity = scopeIdentity;
    }

    public async Task SendAsync<TCommand>(TCommand command) where TCommand : class, IInternalMessage
    {
        command.Token = _scopeIdentity.GetToken();
        await _commandBus.SendAsync(command);
    }

    public async Task<IResult> ExecuteAsync<TCommand>(TCommand command) where TCommand : class, ICommand
    {
        return await _commandExecutor.ExecuteAsync(command);
    }

    public async Task<IResult<TResponse>> ExecuteAsync<TCommand, TResponse>(TCommand command) 
        where TCommand : class, ICommand
        where TResponse : class
    {
        return await _commandExecutor.ExecuteAsync<TCommand, TResponse>(command);
    }
}
