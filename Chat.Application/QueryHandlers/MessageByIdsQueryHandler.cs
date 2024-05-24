using Chat.Application.DTOs;
using Chat.Application.Extensions;
using Chat.Application.Queries;
using Chat.Domain.Repositories;
using Peacious.Framework.CQRS;
using Peacious.Framework.Results;

namespace Chat.Application.QueryHandlers;

public class MessageByIdsQueryHandler : IQueryHandler<MessageByIdsQuery, List<MessageDto>>
{
    private readonly IMessageRepository _messageRepository;

    public MessageByIdsQueryHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<IResult<List<MessageDto>>> HandleAsync(MessageByIdsQuery request)
    {
        var messageIds = request.MessageIds;

        var messages = await _messageRepository.GetMessagesByIds(messageIds);

        var messageDtos = messages.Select(message => message.ToMessageDto()).ToList();

        return Result.Success(messageDtos);
    }
}
