using Chat.Contact.Application.Commands;
using Chat.Contact.Application.Queries;
using Chat.Contact.Domain.Models;
using Chat.Framework.CQRS;
using Chat.Infrastructure.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Contact.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupController : ACommonController
{
    public GroupController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor) 
        : base(commandExecutor, queryExecutor) {}

    [HttpPost, Route("create")]
    public async Task<IActionResult> CreateNewGroupCommandAsync(CreateNewGroupCommand command)
    {
        return Ok(await GetCommandResponseAsync(command));
    }

    [HttpPost, Route("add-memeber")]
    public async Task<IActionResult> AddMemberToGroupAsync(AddMemberToGroupCommand command)
    {
        return Ok(await GetCommandResponseAsync(command));
    }

    [HttpPost, Route("get")]
    public async Task<IActionResult> GroupQueryAsync(GroupQuery query)
    {
        return Ok(await GetQueryResponseAsync<GroupQuery, List<GroupModel>>(query));
    }
}
