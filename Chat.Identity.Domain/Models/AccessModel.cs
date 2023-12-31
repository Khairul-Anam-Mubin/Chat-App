using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Identity.Domain.Models;

public class AccessModel : IEntity
{
    public string Id { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string AppId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool Expired { get; set; }
}