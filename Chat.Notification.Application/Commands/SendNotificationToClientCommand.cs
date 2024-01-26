using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Entities;

namespace Chat.Notification.Application.Commands;

public class SendNotificationToClientCommand : SendNotificationCommand
{
    public SendNotificationToClientCommand(NotificationData notification, List<string> receiverUserIds) 
        : base(notification, receiverUserIds) {}
}