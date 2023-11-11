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

    public IQueryResponse CreateResponse(IQueryResponse response)
    {
        response.Offset = Offset;
        response.Limit = Limit;
        return response;
    }
}