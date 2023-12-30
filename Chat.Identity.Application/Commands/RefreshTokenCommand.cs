using System.ComponentModel.DataAnnotations;
using Chat.Framework.CQRS;
using Chat.Identity.Domain.Models;

namespace Chat.Identity.Application.Commands;

public class RefreshTokenCommand : ICommand
{
    [Required]
    public Token Token { get; set; }

    [Required]
    public string AppId { get; set; } = string.Empty;
}