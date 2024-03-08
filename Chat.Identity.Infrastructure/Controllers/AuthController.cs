using Chat.Framework.CQRS;
using Chat.Identity.Application.Commands;
using Chat.Identity.Application.Dtos;
using Chat.Infrastructure.Shared.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Identity.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ACommonController
{
    public AuthController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
        : base(commandExecutor, queryExecutor) {}

    [HttpPost]
    [Route("log-in")]
    public async Task<IActionResult> LoginUserAsync(LoginCommand command)
    {
        return Ok(await GetCommandResponseAsync<LoginCommand,Token>(command));
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
        return Ok(await GetCommandResponseAsync<RefreshTokenCommand, Token>(command));
    }
}