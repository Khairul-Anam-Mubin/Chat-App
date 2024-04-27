using System.ComponentModel.DataAnnotations;
using Chat.FileStore.Application.DTOs;
using KCluster.Framework.CQRS;
using KCluster.Framework.Pagination;

namespace Chat.FileStore.Application.Queries;

public class FileDownloadQuery : APaginationQuery<FileDownloadResult>, IQuery
{
    [Required]
    public string FileId { get; set; } = string.Empty;
}