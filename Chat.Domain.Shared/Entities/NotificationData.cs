using Chat.Domain.Shared.Constants;

namespace Chat.Domain.Shared.Entities;

public class NotificationData
{
    public string Id { get; set; }
    public NotificationType NotificationType { get; set; }
    public object? Content { get; set; }
    public string ContentType { get; set; }
    public string Sender { get; set; }
    
    public NotificationData(NotificationType notificationType, object content, string contentType, string sender)
    {
        Id = Guid.NewGuid().ToString();
        NotificationType = notificationType;
        Content = content;
        ContentType = contentType;
        Sender = sender;
    }
}