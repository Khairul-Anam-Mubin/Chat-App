using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Results;

namespace Chat.FileStore.Domain.Entities;

public class FileModel : IEntity
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; private set; }
    public string Extension { get; private set; }
    public string Url { get; private set; }
    public DateTime UploadedAt { get; private set; }
    public string UserId { get; private set; }

    private FileModel(string id, string name, string extenstion, string url, string userId)
    {
        Id = id;
        Name = name;
        Extension = extenstion;
        Url = url;
        UserId = userId;
        UploadedAt = DateTime.UtcNow;
    }

    public static IResult<FileModel> Create(string id, string name, string extenstion, string url, string userId)
    {
        return Result.Success(new FileModel(id, name, extenstion, url, userId));
    }
}