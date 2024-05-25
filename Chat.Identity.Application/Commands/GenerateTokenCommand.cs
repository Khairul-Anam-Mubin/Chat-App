using Chat.Identity.Application.Dtos;
using Peacious.Framework.CQRS;

namespace Chat.Identity.Application.Commands;

public class GenerateTokenCommand : ICommand<TokenDto>
{
    public string UserId { get; set; }
    public string AppId { get; set; }
}
