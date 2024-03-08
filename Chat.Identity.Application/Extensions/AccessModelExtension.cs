using Chat.Identity.Application.Dtos;
using Chat.Identity.Domain.Entities;

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