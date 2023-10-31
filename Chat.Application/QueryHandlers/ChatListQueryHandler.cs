using Chat.Application.Extensions;
using Chat.Application.Interfaces;
using Chat.Domain.Queries;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.QueryHandlers;

[ServiceRegister(typeof(IRequestHandler<ChatListQuery, QueryResponse>), ServiceLifetime.Singleton)]
public class ChatListQueryHandler : AQueryHandler<ChatListQuery, QueryResponse>
{
    private readonly ILatestChatRepository _latestChatRepository;

    public ChatListQueryHandler(ILatestChatRepository latestChatRepository)
    {
        _latestChatRepository = latestChatRepository;
    }

    protected override async Task<QueryResponse> OnHandleAsync(ChatListQuery query)
    {
        var response = query.CreateResponse();

        var latestChatModels = await _latestChatRepository.GetLatestChatModelsAsync(query.UserId, query.Offset, query.Limit);
        foreach (var latestChatModel in latestChatModels)
        {
            response.AddItem(latestChatModel.ToLatestChatDto(query.UserId));
        }

        return (QueryResponse)response;
    }
}