using Chat.Domain.Shared.Entities;
using Chat.Framework.CQRS;

namespace Chat.Domain.Shared.Commands;

public class SendNotificationCommand : ICommand
{
    public Notification? Notification { get; set; }
    public NotificationReceiver? Receiver { get; set; }
}