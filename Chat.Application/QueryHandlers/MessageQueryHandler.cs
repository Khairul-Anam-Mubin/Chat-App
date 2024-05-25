using Chat.Application.DTOs;
using Chat.Application.Extensions;
using Chat.Application.Queries;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Peacious.Framework.CQRS;
using Peacious.Framework.Identity;
using Peacious.Framework.Pagination;
using Peacious.Framework.Results;

namespace Chat.Application.QueryHandlers;

public class MessageQueryHandler : IQueryHandler<MessageQuery, IPaginationResponse<MessageDto>>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IScopeIdentity _scopeIdentity;

    public MessageQueryHandler(IMessageRepository messageRepository, IScopeIdentity scopeIdentity)
    {
        _messageRepository = messageRepository;
        _scopeIdentity = scopeIdentity;
    }

    public Task<IResult<IPaginationResponse<MessageDto>>> Handle(MessageQuery request, CancellationToken cancellationToken)
    {
        return HandleAsync(request);
    }

    public async Task<IResult<IPaginationResponse<MessageDto>>> HandleAsync(MessageQuery query)
    {
        var senderId = _scopeIdentity.GetUserId()!;

        List<Message> messages;

        if (query.IsGroupMessage)
        {
            messages = await _messageRepository.GetGroupMessagesAsync(query.ReceiverId, query.Offset, query.Limit);
        }
        else
        {
            var conversationId = Conversation.GetConversationId(senderId, query.ReceiverId);

            messages =
                await _messageRepository.GetConversationMessagesAsync(conversationId, query.Offset, query.Limit);
        }

        var response = query.CreateResponse();

        foreach (var message in messages)
        {
            response.AddItem(message.ToMessageDto());   
        }

        return Result.Success(response);
    }
}