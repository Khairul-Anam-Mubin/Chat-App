namespace Chat.Framework.RequestResponse;

public interface IPaginationResponse<TItem> : IResponse
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