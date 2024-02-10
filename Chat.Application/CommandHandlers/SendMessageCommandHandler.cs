using Chat.Application.Commands;
using Chat.Application.DTOs;
using Chat.Application.Extensions;
using Chat.Domain.Interfaces;
using Chat.Domain.Models;
using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Entities;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Results;

namespace Chat.Application.CommandHandlers;

public class SendMessageCommandHandler : ICommandHandler<SendMessageCommand, ChatDto>
{
    private readonly IChatRepository _chatRepository;
    private readonly ICommandExecutor _commandExecutor;
    private readonly ICommandService _commandService;
    private readonly IScopeIdentity _scopIdentity; 

    public SendMessageCommandHandler(
        IChatRepository chatRepository,
        ICommandExecutor commandExecutor,
        IScopeIdentity scopeIdentity,
        ICommandService commandService)
    {
        _chatRepository = chatRepository;
        _commandExecutor = commandExecutor;
        _scopIdentity = scopeIdentity;
        _commandService = commandService;
    }

    public async Task<IResult<ChatDto>> HandleAsync(SendMessageCommand command)
    { 
        var chatModel = command.ChatModel;

        if (chatModel is null)
        {
            return Result.Error<ChatDto>("ChatModel not set");
        }

        chatModel.Id = Guid.NewGuid().ToString();
        chatModel.SentAt = DateTime.UtcNow;
        chatModel.Status = "Sent";
        chatModel.UserId = _scopIdentity.GetUserId()!;

        if (!await _chatRepository.SaveAsync(chatModel))
        {
            return Result.Error<ChatDto>("ChatModel Creation Failed");
        }

        if (chatModel.IsGroupMessage)
        {
            await SendForHandleGroupChatAsync(chatModel);
        }
        else
        {
            await SendChatNotificationAsync(chatModel);
        }

        await UpdateLatestChatModelAsync(chatModel);
        
        return Result.Success(chatModel.ToChatDto());
    }

    private Task SendChatNotificationAsync(ChatModel chatModel)
    {
        var notification = new NotificationData(GetUserChatTopic(chatModel), chatModel, "ChatModel", chatModel.UserId)
        {
            Id = chatModel.Id
        };

        var sendNotificationCommand = new SendNotificationCommand(notification, new List<string> { chatModel.SendTo, chatModel.UserId });
        
        return _commandService.SendAsync(sendNotificationCommand);
    }

    private Task UpdateLatestChatModelAsync(ChatModel chatModel)
    {
        var latestChatModel = chatModel.ToLatestChatModel();

        var updateLatestChatCommand = new UpdateLatestChatCommand
        {
            LatestChatModel = latestChatModel
        };

        return _commandService.ExecuteAsync(updateLatestChatCommand);
    }

    private Task SendForHandleGroupChatAsync(ChatModel chatModel)
    {
        var handleGroupChatCommand = 
            new HandleGroupChatCommand(chatModel.SendTo, chatModel.UserId, chatModel.Id);
        return _commandService.SendAsync(handleGroupChatCommand);
    }

    private string GetUserChatTopic(ChatModel chatModel)
    {
        var ids = new List<string> { chatModel.UserId, chatModel.SendTo };
        ids.Sort();
        return $"UserChat-{ids[0]}-{ids[1]}";
    }
}