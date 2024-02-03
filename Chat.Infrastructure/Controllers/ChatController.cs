using Chat.Application.Commands;
using Chat.Application.DTOs;
using Chat.Application.Queries;
using Chat.Framework.CQRS;
using Chat.Framework.Pagination;
using Chat.Infrastructure.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : ACommonController
{

    public ChatController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor) 
        : base(commandExecutor, queryExecutor) {}

    [HttpPost, Route("send")]
    public async Task<IActionResult> SendMessageAsync(SendMessageCommand command)
    {
        return Ok(await GetCommandResponseAsync<SendMessageCommand, ChatDto>(command));
    }

    [HttpPost, Route("update-status")]
    public async Task<IActionResult> UpdateChatsStatusAsync(UpdateChatsStatusCommand command)
    {
        return Ok(await GetCommandResponseAsync(command));
    }

    [HttpPost, Route("list")]
    public async Task<IActionResult> GetChatListAsync(ChatListQuery query)
    {
        return Ok(await GetQueryResponseAsync<ChatListQuery, IPaginationResponse<LatestChatDto>>(query));
    }

    [HttpPost, Route("get")]
    public async Task<IActionResult> GetChatsAsync(ChatQuery query)
    {
        return Ok(await GetQueryResponseAsync<ChatQuery, IPaginationResponse<ChatDto>>(query));
    }

    [HttpPost, Route("chats-by-ids")]
    public async Task<IActionResult> GetChatsByIds(ChatByIdsQuery query)
    {
        return Ok(await GetQueryResponseAsync<ChatByIdsQuery, List<ChatDto>>(query));
    }
}