using Chat.Application.DTOs;
using Chat.Application.Extensions;
using Chat.Application.Queries;
using Chat.Domain.Interfaces;
using Chat.Framework.CQRS;
using Chat.Framework.Results;

namespace Chat.Application.QueryHandlers;

public class ChatByIdsQueryHandler : IQueryHandler<ChatByIdsQuery, List<ChatDto>>
{
    private readonly IChatRepository _chatRepository;

    public ChatByIdsQueryHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<IResult<List<ChatDto>>> HandleAsync(ChatByIdsQuery request)
    {
        var chatIds = request.ChatIds;

        var chatModels = await _chatRepository.GetChatModelsByIds(chatIds);

        var chatDtos = chatModels.Select(chatModel => chatModel.ToChatDto()).ToList();

        return Result.Success(chatDtos);
    }
}
