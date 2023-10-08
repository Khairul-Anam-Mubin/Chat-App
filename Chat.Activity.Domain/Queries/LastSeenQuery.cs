using Chat.Framework.CQRS;

namespace Chat.Activity.Domain.Queries;

public class LastSeenQuery : AQuery
{
    public List<string> UserIds { get; set; }

    public LastSeenQuery()
    {
        UserIds = new List<string>();
    }

    public override void ValidateQuery()
    {
        if (UserIds == null || !UserIds.Any())
        {
            throw new Exception("UserId not set");
        }
    }
}