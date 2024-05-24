using Chat.Application.Commands;
using Chat.Application.DTOs;
using Chat.Application.Queries;
using Peacious.Framework.CQRS;
using Peacious.Framework.Pagination;
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
        return Ok(await GetCommandResponseAsync(command));
    }

    [HttpPost, Route("update-status")]
    public async Task<IActionResult> UpdateChatsStatusAsync(UpdateMessageStatusCommand command)
    {
        return Ok(await GetCommandResponseAsync(command));
    }

    [HttpPost, Route("list")]
    public async Task<IActionResult> GetChatListAsync(ConversationsQuery query)
    {
        return Ok(await GetQueryResponseAsync<ConversationsQuery, IPaginationResponse<ConversationDto>>(query));
    }

    [HttpPost, Route("get")]
    public async Task<IActionResult> GetChatsAsync(MessageQuery query)
    {
        return Ok(await GetQueryResponseAsync<MessageQuery, IPaginationResponse<MessageDto>>(query));
    }

    [HttpPost, Route("chats-by-ids")]
    public async Task<IActionResult> GetChatsByIds(MessageByIdsQuery query)
    {
        return Ok(await GetQueryResponseAsync<MessageByIdsQuery, List<MessageDto>>(query));
    }
}