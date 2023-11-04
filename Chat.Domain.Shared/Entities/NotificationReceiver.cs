namespace Chat.Domain.Shared.Entities;

public class NotificationReceiver
{
    public List<string> ReceiverIds { get; set; }
    public string GroupId { get; set; } = string.Empty;

    public NotificationReceiver()
    {
        ReceiverIds = new List<string>();
    }

    public bool IsGroup() => !string.IsNullOrEmpty(GroupId);
}