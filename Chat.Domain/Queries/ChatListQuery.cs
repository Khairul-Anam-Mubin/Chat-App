using Chat.Framework.CQRS;

namespace Chat.Domain.Queries;

public class ChatListQuery : AQuery
{
    public string UserId {get; set;} = string.Empty;
}