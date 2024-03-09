using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Domain.Entities;

public class LatestChatModel : IEntity
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string SendTo { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsGroupMessage { get; set; }
    public int Occurrence { get; set; }
}