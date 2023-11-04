namespace Chat.Framework.Database.Models;

public class PubSubMessage
{
    public string Id { get; set; } = string.Empty;
    public MessageType MessageType { get; set; }
    public object? Message { get; set; }
}

public enum MessageType
{
    Notification = 0
}