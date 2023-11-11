using Chat.FileStore.Application.DTOs;
using Chat.Framework.RequestResponse;

namespace Chat.FileStore.Application.Queries;

public class FileDownloadQuery : PaginationQuery<FileDownloadResult>
{
    public string FileId { get; set; } = string.Empty;
}