using Chat.Domain.Shared.Constants;

namespace Chat.Domain.Shared.Entities;

public class NotificationData
{
    public string Id { get; set; } = string.Empty;
    public NotificationType NotificationType { get; set; }
    public object? Content { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public string Sender { get; set; } = string.Empty;
}