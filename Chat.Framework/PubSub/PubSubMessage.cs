namespace Chat.Framework.PubSub;

public class PubSubMessage
{
    public string Id { get; set; } = string.Empty;
    public MessageType MessageType { get; set; }
    public object? Message { get; set; }
    public string? Token { get; set; }
}