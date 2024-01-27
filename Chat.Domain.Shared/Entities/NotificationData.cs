using Chat.Domain.Shared.Constants;

namespace Chat.Domain.Shared.Entities;

public class NotificationData
{
    public string Id { get; set; }
    public string Topic { get; set; }
    public object? Content { get; set; }
    public string ContentType { get; set; }
    public string Sender { get; set; }
    
    public NotificationData(string topic, object content, string contentType, string sender)
    {
        Id = Guid.NewGuid().ToString();
        Topic = topic;
        Content = content;
        ContentType = contentType;
        Sender = sender;
    }
}