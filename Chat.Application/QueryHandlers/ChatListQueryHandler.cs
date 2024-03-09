using Chat.Application.DTOs;
using Chat.Application.Extensions;
using Chat.Application.Queries;
using Chat.Domain.Repositories;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.Pagination;
using Chat.Framework.Results;

namespace Chat.Application.QueryHandlers;

public class ChatListQueryHandler : IQueryHandler<ChatListQuery, IPaginationResponse<LatestChatDto>>
{
    private readonly ILatestChatRepository _latestChatRepository;
    private readonly IScopeIdentity _scopeIdentity;
    public ChatListQueryHandler(ILatestChatRepository latestChatRepository, IScopeIdentity scopeIdentity)
    {
        _latestChatRepository = latestChatRepository;
        _scopeIdentity = scopeIdentity;
    }

    public async Task<IResult<IPaginationResponse<LatestChatDto>>> HandleAsync(ChatListQuery query)
    {
        var userId = _scopeIdentity.GetUserId()!;
        var response = query.CreateResponse();

        var latestChatModels = 
            await _latestChatRepository.GetLatestChatModelsAsync(userId, query.Offset, query.Limit);
        
        foreach (var latestChatModel in latestChatModels)
        {
            response.AddItem(latestChatModel.ToLatestChatDto(userId));
        }

        return Result.Success(response);
    }
}