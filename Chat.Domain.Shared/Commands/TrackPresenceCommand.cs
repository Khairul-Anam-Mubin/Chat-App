using System.ComponentModel.DataAnnotations;
using Chat.Framework.CQRS;

namespace Chat.Domain.Shared.Commands;

public class TrackPresenceCommand : ICommand, IInternalMessage
{
    public string? Token { get ; set ; }
}