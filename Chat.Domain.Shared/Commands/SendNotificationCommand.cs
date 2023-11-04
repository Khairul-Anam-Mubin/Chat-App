using Chat.Domain.Shared.Entities;

namespace Chat.Domain.Shared.Commands;

public class SendNotificationCommand
{
    public Notification? Notification { get; set; }
    public NotificationReceiver? Receiver { get; set; }
}