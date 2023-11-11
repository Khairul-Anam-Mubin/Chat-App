using Chat.Application.DTOs;
using Chat.Application.Extensions;
using Chat.Application.Queries;
using Chat.Domain.Interfaces;
using Chat.Framework.Attributes;
using Chat.Framework.Mediators;
using Chat.Framework.RequestResponse;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.QueryHandlers;

[ServiceRegister(typeof(IHandler<ChatListQuery, IPaginationResponse<LatestChatDto>>), ServiceLifetime.Singleton)]
public class ChatListQueryHandler : 
    IHandler<ChatListQuery, IPaginationResponse<LatestChatDto>>
{
    private readonly ILatestChatRepository _latestChatRepository;

    public ChatListQueryHandler(ILatestChatRepository latestChatRepository)
    {
        _latestChatRepository = latestChatRepository;
    }

    public async Task<IPaginationResponse<LatestChatDto>> HandleAsync(ChatListQuery query)
    {
        var response = query.CreateResponse();

        var latestChatModels = await _latestChatRepository.GetLatestChatModelsAsync(query.UserId, query.Offset, query.Limit);
        foreach (var latestChatModel in latestChatModels)
        {
            response.AddItem(latestChatModel.ToLatestChatDto(query.UserId));
        }

        return response;
    }
}