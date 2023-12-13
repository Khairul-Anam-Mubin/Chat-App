using Chat.Activity.Application.DTOs;
using Chat.Activity.Application.Queries;
using Chat.Domain.Shared.Commands;
using Chat.Framework.CQRS;
using Chat.Infrastructure.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Activity.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ActivityController : ACommonController
{
    public ActivityController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor) 
        : base(commandExecutor, queryExecutor) {}

    [HttpPost, Route("last-seen")]
    public async Task<IActionResult> GetLastSeenModelAsync(LastSeenQuery query)
    {
        return Ok(await GetQueryResponseAsync<LastSeenQuery, List<LastSeenDto>>(query));
    }

    [HttpPost, Route("track")]
    public async Task<IActionResult> UpdateLastSeenAsync(UpdateLastSeenCommand command)
    {
        return Ok(await GetCommandResponseAsync(command));
    }
}