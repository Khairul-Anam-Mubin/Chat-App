using Chat.Framework.Models;

namespace Chat.Framework.CQRS;

public abstract class AQuery : MetaDataDictionary, IQuery
{
    public int Offset { get; set; }
    public int Limit { get; set; }

    protected AQuery()
    {
        Offset = 0;
        Limit = 1;
    }

    public IQueryResponse CreateResponse()
    {
        return new QueryResponse
        {
            Offset = Offset,
            Limit = Limit
        };
    }

    public IQueryResponse<TItem> CreateResponse<TItem>()
    {
        return new QueryResponse<TItem>
        {
            Offset = Offset,
            Limit = Limit
        };
    }
}