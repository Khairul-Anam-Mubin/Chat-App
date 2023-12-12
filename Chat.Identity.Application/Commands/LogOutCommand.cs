using Chat.Framework.CQRS;

namespace Chat.Identity.Application.Commands;

public class LogOutCommand : ICommand
{
    public string AppId { get; set; } = string.Empty;
}