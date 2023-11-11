using Chat.Framework.RequestResponse;

namespace Chat.Framework.CQRS;

public interface IQueryResponse : IQueryResponse<object> {}

public interface IQueryResponse<TItem> : IResponse
{
    int Offset { get; set; }
    int Limit { get; set; }
    int TotalCount { get; set; }

    List<TItem> Items { get; set; }

    void AddItem(TItem item);

    void AddItems(List<TItem> items);

    void SetItems(List<TItem> items);

    List<TItem> GetItems();
}