using Chat.Identity.Domain.Models;

namespace Chat.Identity.Application.Extensions;

public static class AccessModelExtension
{
    public static Token ToToken(this AccessModel accessModel)
    {
        return new Token
        {
            RefreshToken = accessModel.Id,
            AccessToken = accessModel.AccessToken,
        };
    }
}