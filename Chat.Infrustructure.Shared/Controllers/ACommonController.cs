using Chat.Framework.CQRS;
using Chat.Framework.Results;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Infrastructure.Shared.Controllers;

public abstract class ACommonController : ControllerBase
{
    protected readonly ICommandExecutor CommandExecutor;

    protected ACommonController(ICommandExecutor commandExecutor)
    {
        CommandExecutor = commandExecutor;
    }

    protected async Task<IResult> GetCommandResponseAsync<TCommand>(TCommand command) where TCommand : class, ICommand
    {
        return await CommandExecutor.ExecuteAsync(command);
    }
}