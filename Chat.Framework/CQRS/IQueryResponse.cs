using Chat.Framework.Interfaces;

namespace Chat.Framework.CQRS;

public interface IQueryResponse : IResponse
{
    int Offset { get; set; }
    int Limit { get; set; }
    int TotalCount { get; set; }
    List<object> Items { get; set; }

    void AddItem(object item);

    void AddItems(List<object> items);

    void SetItems(List<object> items);

    List<T> GetItems<T>();
}