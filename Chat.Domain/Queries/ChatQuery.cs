using Chat.Framework.CQRS;

namespace Chat.Domain.Queries;

public class ChatQuery : AQuery
{
    public string UserId {get; set;} = string.Empty;
    public string SendTo {get; set;} = string.Empty;
}