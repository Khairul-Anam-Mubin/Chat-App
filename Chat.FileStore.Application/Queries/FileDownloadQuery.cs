using Chat.FileStore.Application.DTOs;
using Chat.Framework.RequestResponse;

namespace Chat.FileStore.Application.Queries;

public class FileDownloadQuery : APaginationQuery<FileDownloadResult>
{
    public string FileId { get; set; } = string.Empty;
}