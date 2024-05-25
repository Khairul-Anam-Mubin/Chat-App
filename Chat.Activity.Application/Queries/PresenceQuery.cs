using Chat.Activity.Application.DTOs;
using Peacious.Framework.CQRS;

namespace Chat.Activity.Application.Queries;

public class PresenceQuery : IQuery<List<PresenceDto>>
{
    public List<string> UserIds { get; set; }

    public PresenceQuery()
    {
        UserIds = new List<string>();
    }
}