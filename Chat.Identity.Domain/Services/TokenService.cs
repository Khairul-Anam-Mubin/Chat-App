using System.Security.Claims;
using System.Text;
using Chat.Domain.Shared.Constants;
using Chat.Domain.Shared.Entities;
using Chat.Framework.Extensions;
using Chat.Framework.Identity;
using Chat.Framework.Results;
using Chat.Identity.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Chat.Identity.Domain.Services;

public class TokenService : ITokenService
{
    private readonly TokenConfig _tokenConfig;

    public TokenService(IConfiguration configuration)
    {
        _tokenConfig = configuration.TryGetConfig<TokenConfig>();
    }

    public IResult CheckForValidRefreshTokenRequest(string jwtToken)
    {
        if (!TokenHelper.TryValidateToken(
            jwtToken,
            GetTokenValidationParameters(
                true,
                true,
                false),
            out var validationMessage))
        {
            return Result.Error(validationMessage);
        }

        if (!TokenHelper.IsExpired(jwtToken))
        {
            return Result.Error("AccessToken not expired yet!");
        }

        return Result.Success();
    }

    public AccessModel GenerateAccessModel(UserProfile userProfile, string appId)
    {
        var accessToken = GenerateAccessToken(userProfile, appId);

        var refreshToken = TokenHelper.GenerateRefreshToken();

        var accessModelCreateResult =
            AccessModel.Create(refreshToken, accessToken, userProfile.Id, appId);

        return accessModelCreateResult.Value!;
    }

    public string GenerateAccessToken(UserProfile userProfile, string appId)
    {
        var claims = GenerateClaims(userProfile, appId);

        var accessToken = GenerateAccessToken(claims);

        return accessToken;
    }

    public string GenerateAccessToken(List<Claim> claims)
    {
        var accessToken = TokenHelper.GenerateJwtToken(
            _tokenConfig.Issuer,
            _tokenConfig.Audience,
            _tokenConfig.SecretKey,
            _tokenConfig.ExpirationTimeInSec,
            claims);

        return accessToken;
    }

    public List<Claim> GenerateClaims(UserProfile userProfile, string appId)
    {
        var claims = new List<Claim>
        {
            new Claim(UserClaims.UserId, userProfile.Id),
            new Claim(UserClaims.ProfilePictureId, userProfile.ProfilePictureId),
            new Claim(UserClaims.Email, userProfile.Email),
            new Claim(UserClaims.FirstName, userProfile.FirstName),
            new Claim(UserClaims.LastName, userProfile.LastName),
            new Claim(UserClaims.UserName, userProfile.UserName),
            new Claim(UserClaims.AppId, appId),
            new Claim(UserClaims.JwtId, Guid.NewGuid().ToString())
        };

        return claims;
    }

    public TokenValidationParameters GetTokenValidationParameters(
        bool validateIssuer = true,
        bool validateAudience = true,
        bool validateLifetime = true,
        bool validateIssuerSigningKey = true)
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = validateIssuer,
            ValidateAudience = validateAudience,
            ValidateLifetime = validateLifetime,
            ValidateIssuerSigningKey = validateIssuerSigningKey,

            ClockSkew = TimeSpan.Zero,
            ValidIssuer = _tokenConfig.Issuer,
            ValidAudience = _tokenConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(_tokenConfig.SecretKey))
        };
    }
}