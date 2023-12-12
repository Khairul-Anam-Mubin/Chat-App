using Chat.Framework.CQRS;

namespace Chat.Activity.Application.Queries;

public class LastSeenQuery : IQuery
{
    public List<string> UserIds { get; set; }

    public LastSeenQuery()
    {
        UserIds = new List<string>();
    }
}