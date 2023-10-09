using Chat.Framework.CQRS;
using Chat.Identity.Domain.Models;

namespace Chat.Identity.Domain.Commands;

public class RefreshTokenCommand : ACommand
{
    public Token Token { get; set; }
    public string AppId { get; set; } = string.Empty;
}