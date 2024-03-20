using Chat.Identity.Application.Dtos;

namespace Chat.Identity.Application.Extensions;

public static class TokenExtension
{
    public static TokenDto ToTokenDto(this Domain.Entities.Token token)
    {
        return new TokenDto
        {
            RefreshToken = token.Id,
            AccessToken = token.AccessToken,
        };
    }
}