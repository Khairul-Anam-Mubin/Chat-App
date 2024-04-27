using KCluster.Framework.CQRS;
using KCluster.Framework.EmailSenders;
using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.Shared.Commands;

public class SendEmailCommand : ICommand, IInternalMessage
{
    [Required]
    public Email Email { get; set; }
    public string? Token { get; set; }

    public SendEmailCommand()
    {
        Email = new Email();
    }
}