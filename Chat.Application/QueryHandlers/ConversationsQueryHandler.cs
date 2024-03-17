using Chat.Application.DTOs;
using Chat.Application.Extensions;
using Chat.Application.Queries;
using Chat.Domain.Repositories;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.Pagination;
using Chat.Framework.Results;

namespace Chat.Application.QueryHandlers;

public class ConversationsQueryHandler : IQueryHandler<ConversationsQuery, IPaginationResponse<ConversationDto>>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IScopeIdentity _scopeIdentity;

    public ConversationsQueryHandler(IConversationRepository conversationRepository, IScopeIdentity scopeIdentity)
    {
        _conversationRepository = conversationRepository;
        _scopeIdentity = scopeIdentity;
    }

    public async Task<IResult<IPaginationResponse<ConversationDto>>> HandleAsync(ConversationsQuery query)
    {
        var userId = _scopeIdentity.GetUserId()!;
        var response = query.CreateResponse();

        var conversations = 
            await _conversationRepository.GetConversationsAsync(userId, query.Offset, query.Limit);
        
        foreach (var conversation in conversations)
        {
            response.AddItem(conversation.ToConversationDto(userId));
        }

        return Result.Success(response);
    }
}