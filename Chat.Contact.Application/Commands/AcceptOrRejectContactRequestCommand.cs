using KCluster.Framework.CQRS;
using System.ComponentModel.DataAnnotations;

namespace Chat.Contacts.Application.Commands;

public class AcceptOrRejectContactRequestCommand : ICommand
{
    [Required]
    public string ContactId { get; set; } = string.Empty;
    public bool IsAcceptRequest { get; set; }
}