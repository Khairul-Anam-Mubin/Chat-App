using Chat.Framework.CQRS;

namespace Chat.FileStore.Domain.Queries;

public class FileDownloadQuery : AQuery
{
    public string FileId { get; set; } = string.Empty;
}