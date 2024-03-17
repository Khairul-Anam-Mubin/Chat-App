using Chat.Application.Commands;
using Chat.Application.Extensions;
using Chat.Domain.DomainEvents;
using Chat.Domain.Entities;
using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Entities;
using Chat.Framework.CQRS;
using Chat.Framework.DDD;

namespace Chat.Application.EventHandlers;

public class MessageCreatedDomainEventHandler : IDomainEventHandler<MessageCreatedDomainEvent>
{
    private readonly ICommandService _commandService;
    
    public MessageCreatedDomainEventHandler(ICommandService commandService)
    {
        _commandService = commandService;
    }

    public async Task Handle(MessageCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var message = notification.Message;

        if (message.IsGroupMessage)
        {
            await SendForHandleGroupMessageAsync(message);
        }
        else
        {
            await SendMessageNotificationAsync(message);
        }

        await UpdateConversationAsync(message);
    }

    private Task SendMessageNotificationAsync(Message message)
    {
        var notification = new NotificationData(GetUserMessageTopic(message), message.ToMessageDto(), "Chat", message.UserId)
        {
            Id = message.Id
        };

        var sendNotificationCommand = new SendNotificationCommand(notification, new List<string> { message.SendTo, message.UserId });

        return _commandService.SendAsync(sendNotificationCommand);
    }

    private Task UpdateConversationAsync(Message message)
    {
        var latestChat = message.ToConversation();

        var updateConversationCommand = new UpdateConversationCommand
        {
            Conversation = latestChat
        };

        return _commandService.ExecuteAsync(updateConversationCommand);
    }

    private Task SendForHandleGroupMessageAsync(Message message)
    {
        var handleGroupMessageCommand =
            new HandleGroupMessageCommand(message.SendTo, message.UserId, message.Id);

        return _commandService.SendAsync(handleGroupMessageCommand);
    }

    private string GetUserMessageTopic(Message message)
    {
        var ids = new List<string> { message.UserId, message.SendTo };
        ids.Sort();
        return $"UserChat-{ids[0]}-{ids[1]}";
    }
}
