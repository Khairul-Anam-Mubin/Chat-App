using System.ComponentModel.DataAnnotations;
using Chat.FileStore.Domain.Entities;
using Peacious.Framework.CQRS;
using Peacious.Framework.Pagination;

namespace Chat.FileStore.Application.Queries;

public class FileDirectoryQuery : APaginationQuery<FileDirectory>, IQuery
{
    [Required]
    public string FileId { get; set; } = string.Empty;
}