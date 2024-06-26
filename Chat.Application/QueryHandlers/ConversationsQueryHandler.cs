using Chat.Application.DTOs;
using Chat.Application.Extensions;
using Chat.Application.Queries;
using Chat.Domain.Repositories;
using Peacious.Framework.CQRS;
using Peacious.Framework.Identity;
using Peacious.Framework.Pagination;
using Peacious.Framework.Results;

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

    public Task<IResult<IPaginationResponse<ConversationDto>>> Handle(ConversationsQuery request, CancellationToken cancellationToken)
    {
        return HandleAsync(request);
    }

    public async Task<IResult<IPaginationResponse<ConversationDto>>> HandleAsync(ConversationsQuery query)
    {
        var userId = _scopeIdentity.GetUserId()!;

        var conversations = 
            await _conversationRepository.GetUserConversationsAsync(userId, query.Offset, query.Limit);

        var response = query.CreateResponse();

        foreach (var conversation in conversations)
        {
            response.AddItem(conversation.ToConversationDto(userId));
        }

        return Result.Success(response);
    }
}