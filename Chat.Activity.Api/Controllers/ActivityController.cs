using Chat.Activity.Domain.Queries;
using Chat.Domain.Shared.Commands;
using Chat.Framework.CQRS;
using Chat.Framework.Proxy;
using Chat.Presentation.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Activity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ActivityController : ACommonController
{
    private readonly IQueryService _queryService;
    private readonly ICommandService _commandService;

    public ActivityController(
        ICommandService commandService, 
        IQueryService queryService) : 
        base(commandService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpPost, Route("last-seen")]
    public async Task<IActionResult> GetLastSeenModelAsync(LastSeenQuery query)
    {
        return Ok(await _queryService.GetResponseAsync<LastSeenQuery, QueryResponse>(query));
    }

    [HttpPost, Route("track")]
    public async Task<IActionResult> UpdateLastSeenAsync(UpdateLastSeenCommand command)
    {
        return Ok(await _commandService.GetResponseAsync(command));
    }
}