using Peacious.Framework.CQRS;
using System.ComponentModel.DataAnnotations;

namespace Chat.Identity.Application.Commands;

public class VerifyAccountCommand : ICommand
{
    [Required]
    public string UserId { get; set; } = string.Empty;
}