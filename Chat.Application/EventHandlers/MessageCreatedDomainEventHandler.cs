using Chat.Application.Extensions;
using Chat.Domain.DomainEvents;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Entities;
using Chat.Framework.CQRS;
using Chat.Framework.DDD;

namespace Chat.Application.EventHandlers;

public class MessageCreatedDomainEventHandler : IDomainEventHandler<MessageCreatedDomainEvent>
{
    private readonly IMessageRepository _messageRepository;
    private readonly ICommandService _commandService;
    
    public MessageCreatedDomainEventHandler(IMessageRepository messageRepository, ICommandService commandService)
    {
        _messageRepository = messageRepository;
        _commandService = commandService;
    }

    public async Task Handle(MessageCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var message = await _messageRepository.GetByIdAsync(notification.MessageId);

        if (message is null)
        {
            return;
        }

        if (message.IsGroupMessage)
        {
            await SendForHandleGroupMessageAsync(message);
        }
        else
        {
            await SendMessageNotificationAsync(message);
        }
    }

    private Task SendMessageNotificationAsync(Message message)
    {
        var notification = new NotificationData(GetUserMessageTopic(message), message.ToMessageDto(), "Chat", message.SenderId)
        {
            Id = message.Id
        };

        var sendNotificationCommand = new SendNotificationCommand(notification, new List<string> { message.ReceiverId, message.SenderId });

        return _commandService.SendAsync(sendNotificationCommand);
    }

    private Task SendForHandleGroupMessageAsync(Message message)
    {
        var handleGroupMessageCommand =
            new HandleGroupMessageCommand(message.ReceiverId, message.SenderId, message.Id);

        return _commandService.SendAsync(handleGroupMessageCommand);
    }

    private string GetUserMessageTopic(Message message)
    {
        var ids = new List<string> { message.SenderId, message.ReceiverId };
        ids.Sort();
        return $"UserChat-{ids[0]}-{ids[1]}";
    }
}
