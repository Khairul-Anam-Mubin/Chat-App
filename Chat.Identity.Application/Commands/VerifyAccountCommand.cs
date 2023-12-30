using System.ComponentModel.DataAnnotations;
using Chat.Framework.CQRS;

namespace Chat.Identity.Application.Commands;

public class VerifyAccountCommand : ICommand
{
    [Required]
    public string UserId { get; set; } = string.Empty;
}