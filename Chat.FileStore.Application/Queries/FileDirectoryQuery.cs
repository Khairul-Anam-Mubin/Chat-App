using System.ComponentModel.DataAnnotations;
using Chat.FileStore.Domain.Entities;
using KCluster.Framework.CQRS;
using KCluster.Framework.Pagination;

namespace Chat.FileStore.Application.Queries;

public class FileDirectoryQuery : APaginationQuery<FileDirectory>, IQuery
{
    [Required]
    public string FileId { get; set; } = string.Empty;
}