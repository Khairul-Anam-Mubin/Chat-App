﻿using Chat.Contact.Application.Commands;
using Chat.Contact.Application.Queries;
using Chat.Contact.Domain.Entities;
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

    [HttpPost, Route("user-groups")]
    public async Task<IActionResult> UserGroupsQueryAsync(UserGroupsQuery query)
    {
        return Ok(await GetQueryResponseAsync<UserGroupsQuery, List<GroupModel>>(query));
    }

    [HttpPost, Route("groups")]
    public async Task<IActionResult> GroupsQueryAsync(GroupsQuery query)
    {
        return Ok(await GetQueryResponseAsync<GroupsQuery, List<GroupModel>>(query));
    }


    [HttpPost, Route("group-members")]
    public async Task<IActionResult> GroupMembersQueryAsync(GroupMembersQuery query)
    {
        return Ok(await GetQueryResponseAsync<GroupMembersQuery, List<GroupMemberModel>>(query));
    }
}
