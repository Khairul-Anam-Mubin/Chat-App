using Chat.Framework.CQRS;

namespace Chat.Application.Queries;

public class ChatByIdsQuery : IQuery
{
    public List<string> ChatIds { get; set; }

    public ChatByIdsQuery()
    {
        ChatIds = new List<string>();
    }
}
