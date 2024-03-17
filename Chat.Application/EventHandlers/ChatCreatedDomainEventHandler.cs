using Chat.Application.Commands;
using Chat.Application.Extensions;
using Chat.Domain.DomainEvents;
using Chat.Domain.Entities;
using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Entities;
using Chat.Framework.CQRS;
using Chat.Framework.DDD;

namespace Chat.Application.EventHandlers;

public class ChatCreatedDomainEventHandler : IDomainEventHandler<ChatCreatedDomainEvent>
{
    private readonly ICommandService _commandService;
    
    public ChatCreatedDomainEventHandler(ICommandService commandService)
    {
        _commandService = commandService;
    }

    public async Task Handle(ChatCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var chatModel = notification.ChatModel;

        if (chatModel.IsGroupMessage)
        {
            await SendForHandleGroupChatAsync(chatModel);
        }
        else
        {
            await SendChatNotificationAsync(chatModel);
        }

        await UpdateLatestChatModelAsync(chatModel);
    }

    private Task SendChatNotificationAsync(ChatModel chatModel)
    {
        var notification = new NotificationData(GetUserChatTopic(chatModel), chatModel.ToChatDto(), "ChatModel", chatModel.UserId)
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
