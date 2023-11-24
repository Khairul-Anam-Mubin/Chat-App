using Chat.Framework.CQRS;
using Chat.Framework.RequestResponse;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Infrastructure.Shared.Controllers;

public abstract class ACommonController : ControllerBase
{
    protected readonly ICommandExecutor CommandExecutor;

    protected ACommonController(ICommandExecutor commandExecutor)
    {
        CommandExecutor = commandExecutor;
    }

    protected async Task<IResponse> GetCommandResponseAsync<TCommand>(TCommand command) where TCommand : class
    {
        return await CommandExecutor.ExecuteAsync(command);
    }
}