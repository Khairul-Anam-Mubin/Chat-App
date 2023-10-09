using Chat.Framework.CQRS;

namespace Chat.Identity.Domain.Commands;

public class LogOutCommand : ACommand
{
    public string AppId { get; set; } = string.Empty;
}