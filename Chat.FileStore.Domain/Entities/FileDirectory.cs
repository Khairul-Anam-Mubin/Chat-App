using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.DDD;
using Chat.Framework.Results;

namespace Chat.FileStore.Domain.Entities;

public class FileDirectory : Entity, IRepositoryItem
{
    public string Name { get; private set; }
    public string Extension { get; private set; }
    public string Url { get; private set; }
    public DateTime UploadedAt { get; private set; }
    public string UserId { get; private set; }

    private FileDirectory(string id, string name, string extenstion, string url, string userId)
        : base(id)
    {
        Name = name;
        Extension = extenstion;
        Url = url;
        UserId = userId;
        UploadedAt = DateTime.UtcNow;
    }

    public static IResult<FileDirectory> Create(string id, string name, string extenstion, string url, string userId)
    {
        return Result.Success(new FileDirectory(id, name, extenstion, url, userId));
    }
}