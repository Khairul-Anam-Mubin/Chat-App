using Chat.Application.Extensions;
using Chat.Domain.Interfaces;
using Chat.Domain.Queries;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.QueryHandlers;

[ServiceRegister(typeof(IRequestHandler<ChatQuery, IQueryResponse>), ServiceLifetime.Singleton)]
public class ChatQueryHandler : IRequestHandler<ChatQuery, IQueryResponse>
{
    private readonly IChatRepository _chatRepository;

    public ChatQueryHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<IQueryResponse> HandleAsync(ChatQuery query)
    {
        var response = query.CreateResponse();

        var chatModels = await _chatRepository.GetChatModelsAsync(query.UserId, query.SendTo, query.Offset, query.Limit);
        foreach (var chatModel in chatModels)
        {
            response.AddItem(chatModel.ToChatDto());   
        }

        return response;
    }
}