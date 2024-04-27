using KCluster.Framework.CQRS;

namespace Chat.Identity.Application.Commands;

public class GiveDeveloperAccessCommand : ICommand
{
    public string UserId { get; set; }
}
