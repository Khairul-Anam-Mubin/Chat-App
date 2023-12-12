using Chat.Framework.CQRS;

namespace Chat.Identity.Application.Commands;

public class VerifyAccountCommand : ICommand
{
    public string UserId { get; set; } = string.Empty;
}