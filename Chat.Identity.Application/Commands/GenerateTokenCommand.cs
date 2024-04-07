using Chat.Framework.CQRS;

namespace Chat.Identity.Application.Commands;

public class GenerateTokenCommand : ICommand
{
    public string UserId { get; set; }
    public string AppId { get; set; }
}
