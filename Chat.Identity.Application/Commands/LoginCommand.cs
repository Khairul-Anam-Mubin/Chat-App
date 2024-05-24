using Peacious.Framework.CQRS;
using System.ComponentModel.DataAnnotations;

namespace Chat.Identity.Application.Commands;

public class LoginCommand : ICommand
{
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(50),MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string AppId { get; set; } = string.Empty;
}