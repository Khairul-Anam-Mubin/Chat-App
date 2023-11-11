using Chat.Framework.CQRS;
using Chat.Framework.Interfaces;
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

    protected async Task<IResponse> GetCommandResponseAsync<TCommand>(TCommand command) where TCommand : class
    {
        return await CommandService.GetResponseAsync(command);
    }
}