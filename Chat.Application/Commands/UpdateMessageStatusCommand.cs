using System.ComponentModel.DataAnnotations;
using Chat.Framework.CQRS;

namespace Chat.Application.Commands;

public class UpdateMessageStatusCommand : ICommand
{
    [Required]
    public string OpenedChatUserId { get; set; } = string.Empty;
}