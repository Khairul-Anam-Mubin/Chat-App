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

public class SendMessageCommandHandler : ICommandHandler<SendMessageCommand, ChatDto>
{
    private readonly IChatRepository _chatRepository;
    private readonly IScopeIdentity _scopIdentity;
    private readonly IEventService _eventService;

    public SendMessageCommandHandler(
        IChatRepository chatRepository,
        IScopeIdentity scopeIdentity,
        IEventService eventService)
    {
        _chatRepository = chatRepository;
        _scopIdentity = scopeIdentity;
        _eventService = eventService;
    }

    public async Task<IResult<ChatDto>> HandleAsync(SendMessageCommand command)
    {
        var sendTo = command.SendTo;
        var message = command.Message;
        var isGroupMessage = command.IsGroupMessage;

        var chatModelCreateResult =
            ChatModel.Create(_scopIdentity.GetUserId()!, sendTo, message, isGroupMessage);

        if (chatModelCreateResult.IsFailure || chatModelCreateResult.Value is null)
        {
            return (IResult<ChatDto>)chatModelCreateResult;
        }

        var chatModel = chatModelCreateResult.Value;

        if (!await _chatRepository.SaveAsync(chatModel))
        {
            return Result.Error<ChatDto>("ChatModel Creation Failed");
        }

        await _eventService.PublishEventAsync(chatModel.DomainEvents.FirstOrDefault()!);
        
        return Result.Success(chatModel.ToChatDto());
    }
}