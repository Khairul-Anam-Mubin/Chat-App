using Chat.Framework.CQRS;

namespace Chat.Domain.Queries;

public class ChatListQuery : AQuery
{
    public string UserId {get; set;} = string.Empty;

    public override void ValidateQuery()
    {
        if (string.IsNullOrEmpty(UserId))
        {
            throw new Exception("UserId not set");
        }
    }
}