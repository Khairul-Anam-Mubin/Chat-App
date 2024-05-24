using Peacious.Framework.CQRS;
using System.ComponentModel.DataAnnotations;

namespace Chat.Identity.Application.Commands;

public class LogOutCommand : ICommand
{
    [Required]
    public string AppId { get; set; } = string.Empty;
}