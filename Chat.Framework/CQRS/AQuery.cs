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

    public QueryResponse CreateResponse()
    {
        return new QueryResponse
        {
            Name = GetType().Name,
            Offset = Offset,
            Limit = Limit
        };
    }

    public QueryResponse CreateResponse(QueryResponse response)
    {
        response.Name = GetType().Name;
        response.Offset = Offset;
        response.Limit = Limit;
        return response;
    }
}