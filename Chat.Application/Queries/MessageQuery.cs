using System.ComponentModel.DataAnnotations;
using Chat.Application.DTOs;
using KCluster.Framework.CQRS;
using KCluster.Framework.Pagination;

namespace Chat.Application.Queries;

public class MessageQuery : APaginationQuery<MessageDto>, IQuery
{
    [Required]
    public string ReceiverId { get; set; } = string.Empty;

    public bool IsGroupMessage { get; set; }
}