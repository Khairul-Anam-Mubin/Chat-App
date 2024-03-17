using Chat.Application.DTOs;
using Chat.Application.Extensions;
using Chat.Application.Queries;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.Pagination;
using Chat.Framework.Results;

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

    public async Task<IResult<IPaginationResponse<MessageDto>>> HandleAsync(MessageQuery query)
    {
        var userId = _scopeIdentity.GetUserId()!;

        var response = query.CreateResponse();

        List<Message> messages;

        if (query.IsGroupMessage)
        {
            messages = await _messageRepository.GetGroupMessagesAsync(query.SendTo, query.Offset, query.Limit);
        }
        else
        {
            messages =
                await _messageRepository.GetMessagesAsync(userId, query.SendTo, query.Offset, query.Limit);
        }
        
        foreach (var message in messages)
        {
            response.AddItem(message.ToMessageDto());   
        }

        return Result.Success(response);
    }
}