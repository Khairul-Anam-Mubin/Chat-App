using Chat.Application.DTOs;
using Chat.Application.Extensions;
using Chat.Application.Queries;
using Chat.Domain.Interfaces;
using Chat.Framework.CQRS;
using Chat.Framework.Pagination;
using Chat.Framework.Results;

namespace Chat.Application.QueryHandlers;

public class ChatListQueryHandler : IQueryHandler<ChatListQuery, IPaginationResponse<LatestChatDto>>
{
    private readonly ILatestChatRepository _latestChatRepository;

    public ChatListQueryHandler(ILatestChatRepository latestChatRepository)
    {
        _latestChatRepository = latestChatRepository;
    }

    public async Task<IResult<IPaginationResponse<LatestChatDto>>> HandleAsync(ChatListQuery query)
    {
        var response = query.CreateResponse();

        var latestChatModels = 
            await _latestChatRepository.GetLatestChatModelsAsync(query.UserId, query.Offset, query.Limit);
        
        foreach (var latestChatModel in latestChatModels)
        {
            response.AddItem(latestChatModel.ToLatestChatDto(query.UserId));
        }

        return Result.Success(response);
    }
}