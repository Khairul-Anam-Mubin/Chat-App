using Chat.Framework.CQRS;
using Chat.Framework.Proxy;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Presentation.Shared.Controllers;

public abstract class ACommonController : ControllerBase
{
    protected readonly ICommandService CommandService;

    protected ACommonController(ICommandService commandService)
    {
        CommandService = commandService;
    }

    protected async Task<CommandResponse> GetCommandResponseAsync<TCommand>(TCommand command) where TCommand : class, ICommand
    {
        return await CommandService.GetResponseAsync<TCommand, CommandResponse>(command);
    }
}