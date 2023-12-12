using Chat.Framework.CQRS;
using Chat.Identity.Domain.Models;

namespace Chat.Identity.Application.Commands;

public class RefreshTokenCommand : ICommand
{
    public Token Token { get; set; }
    public string AppId { get; set; } = string.Empty;
}