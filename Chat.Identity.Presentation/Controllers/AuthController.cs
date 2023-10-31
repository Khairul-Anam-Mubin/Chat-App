using Chat.Framework.Proxy;
using Chat.Identity.Domain.Commands;
using Chat.Presentation.Shared.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Identity.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ACommonController
{
    public AuthController(ICommandService commandService)
        : base(commandService)
    {

    }

    [HttpPost]
    [Route("log-in")]
    public async Task<IActionResult> LoginUserAsync(LoginCommand command)
    {
        return Ok(await GetCommandResponseAsync(command));
    }

    [HttpPost]
    [Route("log-out")]
    public async Task<IActionResult> LogOutUserAsync(LogOutCommand command)
    {
        return Ok(await GetCommandResponseAsync(command));
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenCommand command)
    {
        return Ok(await GetCommandResponseAsync(command));
    }
}