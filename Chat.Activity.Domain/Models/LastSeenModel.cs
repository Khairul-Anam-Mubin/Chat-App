using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Activity.Domain.Models;

public class LastSeenModel : IEntity
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime LastSeenAt { get; set; }
    public bool IsActive { get; set; }
}