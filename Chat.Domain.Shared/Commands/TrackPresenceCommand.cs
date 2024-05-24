using Peacious.Framework.CQRS;

namespace Chat.Domain.Shared.Commands;

public class TrackPresenceCommand : ICommand, IInternalMessage
{
    public string? Token { get ; set ; }
}