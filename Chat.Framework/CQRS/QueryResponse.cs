using Chat.Framework.RequestResponse;

namespace Chat.Framework.CQRS;

public class QueryResponse : QueryResponse<object>, IQueryResponse {}

public class QueryResponse<TItem> : Response, IQueryResponse<TItem>
{
    public int Offset { get; set; }
    public int Limit { get; set; }
    public int TotalCount { get; set; }
    public List<TItem> Items { get; set; }

    public QueryResponse()
    {
        Items = new List<TItem>();
    }

    public void AddItem(TItem item)
    {
        Items.Add(item);
    }

    public void AddItems(List<TItem> items)
    {
        Items.AddRange(items);
    }

    public void SetItems(List<TItem> items)
    {
        Items = items;
    }

    public List<TItem> GetItems()
    {
        return Items;
    }
}