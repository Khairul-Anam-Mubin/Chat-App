using Chat.Application.DTOs;
using Chat.Domain.Commands;
using Chat.Domain.Queries;
using Chat.Framework.CQRS;
using Chat.Presentation.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : ACommonController
{
    private readonly IQueryExecutor _queryExecutor;

    public ChatController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor) 
        : base(commandExecutor)
    {
        _queryExecutor = queryExecutor;
    }

    [HttpPost, Route("send")]
    public async Task<IActionResult> SendMessageAsync(SendMessageCommand command)
    {
        return Ok(await GetCommandResponseAsync(command));
    }

    [HttpPost, Route("update-status")]
    public async Task<IActionResult> UpdateChatsStatusAsync(UpdateChatsStatusCommand command)
    {
        return Ok(await GetCommandResponseAsync(command));
    }

    [HttpPost, Route("list")]
    public async Task<IActionResult> GetChatListAsync(ChatListQuery query)
    {
        return Ok(await _queryExecutor.ExecuteAsync(query));
    }

    [HttpPost, Route("get")]
    public async Task<IActionResult> GetChatsAsync(ChatQuery query)
    {
        return Ok(await _queryExecutor.ExecuteAsync<ChatQuery, IQueryResponse<ChatDto>>(query));
    }
        
}