using Chat.Domain.Shared.Queries;
using Chat.Framework.CQRS;
using Chat.Identity.Domain.Commands;
using Chat.Presentation.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Identity.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ACommonController
{
    private readonly IQueryExecutor _queryExecutor;

    public UserController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor) : base(commandExecutor)
    {
        _queryExecutor = queryExecutor;
    }

    [HttpPost]
    [Route("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUserAsync(RegisterCommand command)
    {
        return Ok(await GetCommandResponseAsync(command));
    }

    [HttpPost]
    [Route("profiles")]
    public async Task<IActionResult> UserProfileAsync(UserProfileQuery query)
    {
        var response = await _queryExecutor.ExecuteAsync<UserProfileQuery, UserProfileQueryResponse>(query);
        return Ok(response);
    }

    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> UpdateUserAsync(UpdateUserProfileCommand command)
    {
        return Ok(await GetCommandResponseAsync(command));
    }
}