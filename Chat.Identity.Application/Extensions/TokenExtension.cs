using Chat.Identity.Application.Dtos;
using Chat.Identity.Domain.Entities;

namespace Chat.Identity.Application.Extensions;

public static class TokenExtension
{
    public static TokenDto ToTokenDto(this Token token)
    {
        return new TokenDto
        {
            RefreshToken = token.Id,
            AccessToken = token.AccessToken,
        };
    }
}