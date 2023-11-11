using Chat.Framework.Models;

namespace Chat.Framework.RequestResponse;

public abstract class APaginationQuery : 
    MetaDataDictionary, IPaginationQuery
{
    public int Offset { get; set; }
    public int Limit { get; set; }

    protected APaginationQuery()
    {
        Offset = 0;
        Limit = 1;
    }

    public IPaginationResponse CreateResponse()
    {
        return new PaginationResponse
        {
            Offset = Offset,
            Limit = Limit
        };
    }

    public IPaginationResponse<TItem> CreateResponse<TItem>()
    {
        return new PaginationResponse<TItem>
        {
            Offset = Offset,
            Limit = Limit
        };
    }
}

public abstract class PaginationQuery<TItem> : MetaDataDictionary, IPaginationQuery<TItem>
{
    public int Offset { get; set; }
    public int Limit { get; set; }

    public IPaginationResponse<TItem> CreateResponse()
    {
        return new PaginationResponse<TItem>
        {
            Offset = Offset,
            Limit = Limit
        };
    }
}