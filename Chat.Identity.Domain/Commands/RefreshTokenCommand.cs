using Chat.Framework.CQRS;
using Chat.Identity.Domain.Models;

namespace Chat.Identity.Domain.Commands;

public class RefreshTokenCommand : ACommand
{
    public Token Token { get; set; }
    public string AppId { get; set; } = string.Empty;
    public override void ValidateCommand()
    {
        if (Token == null)
        {
            throw new Exception("Token not found!");
        }
        if (string.IsNullOrEmpty(Token.AccessToken))
        {
            throw new Exception("Access Token Not Set!");
        }
        if (string.IsNullOrEmpty(Token.RefreshToken))
        {
            throw new Exception("Refresh Token not set!");
        }
        if (string.IsNullOrEmpty(AppId))
        {
            throw new Exception("AppId not set");
        }
    }
}