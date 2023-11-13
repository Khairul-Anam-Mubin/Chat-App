using Chat.FileStore.Domain.Models;
using Chat.Framework.RequestResponse;

namespace Chat.FileStore.Application.Queries;

public class FileModelQuery : APaginationQuery<FileModel>
{
    public string FileId { get; set; } = string.Empty;
}