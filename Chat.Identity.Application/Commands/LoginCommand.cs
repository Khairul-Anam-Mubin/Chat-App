using Chat.Framework.CQRS;

namespace Chat.Identity.Application.Commands;

public class LoginCommand : ICommand
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string AppId { get; set; } = string.Empty;
}