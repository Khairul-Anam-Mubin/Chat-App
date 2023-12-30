using System.ComponentModel.DataAnnotations;
using Chat.Framework.CQRS;

namespace Chat.Identity.Application.Commands;

public class LogOutCommand : ICommand
{
    [Required]
    public string AppId { get; set; } = string.Empty;
}