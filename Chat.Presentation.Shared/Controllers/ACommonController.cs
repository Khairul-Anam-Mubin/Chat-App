using Chat.Framework.CQRS;
using Chat.Framework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Presentation.Shared.Controllers;

public abstract class ACommonController : ControllerBase
{
    protected readonly ICommandService CommandService;

    protected ACommonController(ICommandService commandService)
    {
        CommandService = commandService;
    }

    protected async Task<Response> GetCommandResponseAsync<TCommand>(TCommand command) where TCommand : class, ICommand
    {
        return await CommandService.GetResponseAsync<TCommand, Response>(command);
    }
}