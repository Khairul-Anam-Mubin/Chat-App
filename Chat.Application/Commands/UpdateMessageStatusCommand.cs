using Peacious.Framework.CQRS;
using System.ComponentModel.DataAnnotations;

namespace Chat.Application.Commands;

public class UpdateMessageStatusCommand : ICommand
{
    [Required]
    public string OpenedChatUserId { get; set; } = string.Empty;
}