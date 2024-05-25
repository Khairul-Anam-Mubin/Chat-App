using Chat.Application.DTOs;
using Peacious.Framework.CQRS;

namespace Chat.Application.Queries;

public class MessageByIdsQuery : IQuery<List<MessageDto>>
{
    public List<string> MessageIds { get; set; }

    public MessageByIdsQuery()
    {
        MessageIds = new List<string>();
    }
}
