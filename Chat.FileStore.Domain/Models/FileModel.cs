using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.FileStore.Domain.Models;

public class FileModel : IEntity
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    public string UserId { get; set; } = string.Empty;
}