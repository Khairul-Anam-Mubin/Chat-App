using System.ComponentModel.DataAnnotations;
using Chat.Domain.Shared.Entities;
using Chat.Framework.CQRS;

namespace Chat.Domain.Shared.Commands;

public class SendNotificationCommand : ICommand
{
    [Required]
    public Notification? Notification { get; set; }

    [Required]
    public NotificationReceiver? Receiver { get; set; }
}