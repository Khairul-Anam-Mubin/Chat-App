using Chat.Framework.CQRS;

namespace Chat.Identity.Domain.Commands;

public class LoginCommand : ACommand
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string AppId { get; set; } = string.Empty;
}