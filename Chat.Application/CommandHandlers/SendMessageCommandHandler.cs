using Chat.Application.Commands;
using Chat.Application.DTOs;
using Chat.Application.Extensions;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Framework.CQRS;
using Chat.Framework.EDD;
using Chat.Framework.Identity;
using Chat.Framework.Results;

namespace Chat.Application.CommandHandlers;

public class SendMessageCommandHandler : ICommandHandler<SendMessageCommand, MessageDto>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IScopeIdentity _scopIdentity;
    private readonly IEventService _eventService;

    public SendMessageCommandHandler(
        IMessageRepository messageRepository,
        IScopeIdentity scopeIdentity,
        IEventService eventService)
    {
        _messageRepository = messageRepository;
        _scopIdentity = scopeIdentity;
        _eventService = eventService;
    }

    public async Task<IResult<MessageDto>> HandleAsync(SendMessageCommand command)
    {
        var sendTo = command.SendTo;
        var messageContent = command.MessageContent;
        var isGroupMessage = command.IsGroupMessage;

        var messageCreateResult =
            Message.Create(_scopIdentity.GetUserId()!, sendTo, messageContent, isGroupMessage);

        if (messageCreateResult.IsFailure || messageCreateResult.Value is null)
        {
            return (IResult<MessageDto>)messageCreateResult;
        }

        var message = messageCreateResult.Value;

        if (!await _messageRepository.SaveAsync(message))
        {
            return Result.Error<MessageDto>("Content Creation Failed");
        }

        await _eventService.PublishEventAsync(message.DomainEvents.FirstOrDefault()!);
        
        return Result.Success(message.ToMessageDto());
    }
}