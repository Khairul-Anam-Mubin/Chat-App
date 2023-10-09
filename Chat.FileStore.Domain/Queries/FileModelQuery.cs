using Chat.Framework.CQRS;

namespace Chat.FileStore.Domain.Queries;

public class FileModelQuery : AQuery
{
    public string FileId { get; set; } = string.Empty;
}