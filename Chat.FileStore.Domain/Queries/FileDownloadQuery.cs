using Chat.Framework.CQRS;

namespace Chat.FileStore.Domain.Queries;

public class FileDownloadQuery : AQuery
{
    public string FileId { get; set; } = string.Empty;
    public override void ValidateQuery()
    {
        if (string.IsNullOrEmpty(FileId))
        {
            throw new Exception("FileId is empty!!");
        }
    }
}