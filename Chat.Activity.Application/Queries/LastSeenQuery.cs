using Chat.Activity.Application.DTOs;
using Chat.Framework.RequestResponse;

namespace Chat.Activity.Application.Queries;

public class LastSeenQuery
{
    public List<string> UserIds { get; set; }

    public LastSeenQuery()
    {
        UserIds = new List<string>();
    }
}

public class LastSeenQueryResponse : Response
{
    public List<LastSeenDto> Items { get; set; }

    public LastSeenQueryResponse()
    {
        Items = new List<LastSeenDto>();
    }
}