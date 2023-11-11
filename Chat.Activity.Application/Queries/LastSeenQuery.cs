using Chat.Activity.Application.DTOs;
using Chat.Framework.RequestResponse;

namespace Chat.Activity.Application.Queries;

public class LastSeenQuery : PaginationQuery<LastSeenDto>
{
    public List<string> UserIds { get; set; }

    public LastSeenQuery()
    {
        UserIds = new List<string>();
    }
}