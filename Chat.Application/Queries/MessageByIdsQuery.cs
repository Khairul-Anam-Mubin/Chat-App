using Chat.Framework.CQRS;

namespace Chat.Application.Queries;

public class MessageByIdsQuery : IQuery
{
    public List<string> MessageIds { get; set; }

    public MessageByIdsQuery()
    {
        MessageIds = new List<string>();
    }
}
