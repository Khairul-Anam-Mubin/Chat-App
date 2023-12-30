using System.ComponentModel.DataAnnotations;
using Chat.FileStore.Domain.Models;
using Chat.Framework.CQRS;
using Chat.Framework.Pagination;

namespace Chat.FileStore.Application.Queries;

public class FileModelQuery : APaginationQuery<FileModel>, IQuery
{
    [Required]
    public string FileId { get; set; } = string.Empty;
}