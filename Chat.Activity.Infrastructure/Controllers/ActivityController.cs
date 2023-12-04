using Chat.Activity.Application.Queries;
using Chat.Domain.Shared.Commands;
using Chat.Framework.CQRS;
using Chat.Framework.RequestResponse;
using Chat.Infrastructure.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Activity.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ActivityController : ACommonController
{
    private readonly IQueryExecutor _queryExecutor;
    private readonly ICommandExecutor _commandExecutor;

    public ActivityController(
        ICommandExecutor commandExecutor,
        IQueryExecutor queryExecutor) :
        base(commandExecutor)
    {
        _commandExecutor = commandExecutor;
        _queryExecutor = queryExecutor;
    }

    [HttpPost, Route("last-seen")]
    public async Task<IActionResult> GetLastSeenModelAsync(LastSeenQuery query)
    {
        return Ok(await _queryExecutor.ExecuteAsync<LastSeenQuery, LastSeenQueryResponse>(query));
    }

    [HttpPost, Route("track")]
    public async Task<IActionResult> UpdateLastSeenAsync(UpdateLastSeenCommand command)
    {
        return Ok(await _commandExecutor.ExecuteAsync<UpdateLastSeenCommand, IResponse>(command));
    }
}