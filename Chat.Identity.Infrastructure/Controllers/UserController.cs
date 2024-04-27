using Chat.Domain.Shared.Constants;
using Chat.Domain.Shared.Entities;
using Chat.Domain.Shared.Queries;
using KCluster.Framework.CQRS;
using KCluster.Framework.Identity;
using Chat.Identity.Application.Commands;
using Chat.Infrastructure.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Identity.Infrastructure.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ACommonController
{
    public UserController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor) 
        : base(commandExecutor, queryExecutor) {}

    [HttpPost]
    [Route("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUserAsync(RegisterCommand command)
    {
        return Ok(await GetCommandResponseAsync(command));
    }

    [HttpPost]
    [Route("profiles")]
    [HasPermission(Permissions.UserRead)]
    public async Task<IActionResult> UserProfileAsync(UserProfileQuery query)
    {
        var response = await GetQueryResponseAsync<UserProfileQuery, UserProfileResponse>(query);
        return Ok(response);
    }

    [HttpPost]
    [Route("update")]
    [HasPermission(Permissions.UserUpdate)]
    public async Task<IActionResult> UpdateUserAsync(UpdateUserProfileCommand command)
    {
        return Ok(await GetCommandResponseAsync<UpdateUserProfileCommand, UserProfile>(command));
    }

    [HttpGet]
    [Route("verify-account")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyAccountAsync([FromQuery] string userId)
    {
        var verifyCommand = new VerifyAccountCommand
        {
            UserId = userId
        };

        var response = await GetCommandResponseAsync(verifyCommand);
        return Ok(response);
    }
}