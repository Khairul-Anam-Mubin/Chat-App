using Chat.Application.DTOs;
using Chat.Application.Extensions;
using Chat.Application.Queries;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.Pagination;
using Chat.Framework.Results;

namespace Chat.Application.QueryHandlers;

public class ChatQueryHandler : IQueryHandler<ChatQuery, IPaginationResponse<ChatDto>>
{
    private readonly IChatRepository _chatRepository;
    private readonly IScopeIdentity _scopeIdentity;

    public ChatQueryHandler(IChatRepository chatRepository, IScopeIdentity scopeIdentity)
    {
        _chatRepository = chatRepository;
        _scopeIdentity = scopeIdentity;
    }

    public async Task<IResult<IPaginationResponse<ChatDto>>> HandleAsync(ChatQuery query)
    {
        var userId = _scopeIdentity.GetUserId()!;

        var response = query.CreateResponse();

        List<ChatModel> chatModels;

        if (query.IsGroupMessage)
        {
            chatModels = await _chatRepository.GetGroupChatModelsAsync(query.SendTo, query.Offset, query.Limit);
        }
        else
        {
            chatModels =
                await _chatRepository.GetChatModelsAsync(userId, query.SendTo, query.Offset, query.Limit);
        }
        
        foreach (var chatModel in chatModels)
        {
            response.AddItem(chatModel.ToChatDto());   
        }

        return Result.Success(response);
    }
}