using Chat.Application.DTOs;
using Chat.Application.Extensions;
using Chat.Application.Queries;
using Chat.Domain.Interfaces;
using Chat.Framework.Attributes;
using Chat.Framework.Mediators;
using Chat.Framework.RequestResponse;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.QueryHandlers;

[ServiceRegister(typeof(IHandler<ChatQuery, IPaginationResponse<ChatDto>>), ServiceLifetime.Singleton)]
public class ChatQueryHandler : 
    IHandler<ChatQuery, IPaginationResponse<ChatDto>>
{
    private readonly IChatRepository _chatRepository;

    public ChatQueryHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<IPaginationResponse<ChatDto>> HandleAsync(ChatQuery query)
    {
        var response = query.CreateResponse();

        var chatModels = 
            await _chatRepository.GetChatModelsAsync(query.UserId, query.SendTo, query.Offset, query.Limit);
        
        foreach (var chatModel in chatModels)
        {
            response.AddItem(chatModel.ToChatDto());   
        }

        return response;
    }
}