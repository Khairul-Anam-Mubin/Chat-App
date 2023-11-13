using Chat.Framework.Models;

namespace Chat.Framework.RequestResponse;

public abstract class APaginationQuery<TItem> : MetaDataDictionary, IPaginationQuery<TItem>
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