using System.ComponentModel.DataAnnotations;
using Chat.FileStore.Application.DTOs;
using Peacious.Framework.CQRS;
using Peacious.Framework.Pagination;

namespace Chat.FileStore.Application.Queries;

public class FileDownloadQuery : APaginationQuery<FileDownloadResult>, IQuery
{
    [Required]
    public string FileId { get; set; } = string.Empty;
}