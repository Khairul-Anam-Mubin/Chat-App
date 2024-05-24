using Peacious.Framework.CQRS;
using System.ComponentModel.DataAnnotations;

namespace Chat.Identity.Application.Commands;

public class RefreshTokenCommand : ICommand
{
    public string AccessToken { get; set; } = string.Empty;
    
    public string RefreshToken { get; set; } = string.Empty;

    [Required]
    public string AppId { get; set; } = string.Empty;
}