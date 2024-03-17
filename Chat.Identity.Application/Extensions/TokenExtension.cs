namespace Chat.Identity.Application.Extensions;

public static class TokenExtension
{
    public static Dtos.TokenDto ToTokenDto(this Domain.Entities.Token token)
    {
        return new Dtos.TokenDto
        {
            RefreshToken = token.Id,
            AccessToken = token.AccessToken,
        };
    }
}