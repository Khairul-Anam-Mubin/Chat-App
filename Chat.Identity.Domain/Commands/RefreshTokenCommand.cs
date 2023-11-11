using Chat.Identity.Domain.Models;

namespace Chat.Identity.Domain.Commands;

public class RefreshTokenCommand
{
    public Token Token { get; set; }
    public string AppId { get; set; } = string.Empty;
}