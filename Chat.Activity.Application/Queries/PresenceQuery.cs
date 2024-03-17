using Chat.Framework.CQRS;

namespace Chat.Activity.Application.Queries;

public class PresenceQuery : IQuery
{
    public List<string> UserIds { get; set; }

    public PresenceQuery()
    {
        UserIds = new List<string>();
    }
}