using Peacious.Framework.CQRS;

namespace Chat.Identity.Application.Commands;

public class GiveDeveloperAccessCommand : ICommand
{
    public string UserId { get; set; }
}
