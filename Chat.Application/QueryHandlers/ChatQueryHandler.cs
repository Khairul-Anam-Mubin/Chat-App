using Chat.Application.DTOs;
using Chat.Application.Extensions;
using Chat.Application.Queries;
using Chat.Domain.Interfaces;
using Chat.Framework.CQRS;
using Chat.Framework.Pagination;
using Chat.Framework.Results;

namespace Chat.Application.QueryHandlers;

public class ChatQueryHandler : IQueryHandler<ChatQuery, IPaginationResponse<ChatDto>>
{
    private readonly IChatRepository _chatRepository;

    public ChatQueryHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<IResult<IPaginationResponse<ChatDto>>> HandleAsync(ChatQuery query)
    {
        var response = query.CreateResponse();

        var chatModels = 
            await _chatRepository.GetChatModelsAsync(query.UserId, query.SendTo, query.Offset, query.Limit);
        
        foreach (var chatModel in chatModels)
        {
            response.AddItem(chatModel.ToChatDto());   
        }

        return Result<IPaginationResponse<ChatDto>>.Success(response);
    }
}