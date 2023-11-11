using Chat.Application.Extensions;
using Chat.Domain.Interfaces;
using Chat.Domain.Queries;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.QueryHandlers;

[ServiceRegister(typeof(IRequestHandler<ChatListQuery, IQueryResponse>), ServiceLifetime.Singleton)]
public class ChatListQueryHandler : IRequestHandler<ChatListQuery, IQueryResponse>
{
    private readonly ILatestChatRepository _latestChatRepository;

    public ChatListQueryHandler(ILatestChatRepository latestChatRepository)
    {
        _latestChatRepository = latestChatRepository;
    }

    public async Task<IQueryResponse> HandleAsync(ChatListQuery query)
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