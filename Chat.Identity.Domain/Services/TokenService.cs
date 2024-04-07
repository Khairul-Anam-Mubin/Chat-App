using System.Security.Claims;
using System.Text;
using Chat.Domain.Shared.Constants;
using Chat.Domain.Shared.Entities;
using Chat.Framework.Identity;
using Chat.Framework.Results;
using Chat.Identity.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Chat.Identity.Domain.Services;

public class TokenService : ITokenService
{
    private readonly TokenConfig _tokenConfig;

    public TokenService(TokenConfig tokenConfig)
    {
        _tokenConfig = tokenConfig;
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

    public Token GenerateToken(User userProfile, UserAccess userAccess, string appId)
    {
        return GenerateToken(userProfile, appId, userAccess.RoleIds, userAccess.PermissionIds);
    }

    public Token GenerateToken(User userProfile, string appId, List<string> roles, List<string> permissions)
    {
        var accessToken = GenerateAccessToken(userProfile, appId, roles, permissions);

        var refreshToken = TokenHelper.GenerateRefreshToken();

        var accessModelCreateResult =
            Token.Create(refreshToken, accessToken, userProfile.Id, appId);

        return accessModelCreateResult.Value!;
    }

    public string GenerateAccessToken(User userProfile, string appId, List<string> roles, List<string> permissions)
    {
        var claims = GenerateClaims(userProfile, appId, roles, permissions);

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

    public List<Claim> GenerateClaims(User userProfile, string appId, List<string> roles, List<string> permissions)
    {
        var claims = new List<Claim>
        {
            new Claim(UserClaims.UserId, userProfile.Id),
            new Claim(UserClaims.ProfilePictureId, userProfile.ProfilePictureId ?? string.Empty),
            new Claim(UserClaims.Email, userProfile.Email),
            new Claim(UserClaims.FirstName, userProfile.FirstName),
            new Claim(UserClaims.LastName, userProfile.LastName),
            new Claim(UserClaims.UserName, userProfile.UserName),
            new Claim(UserClaims.AppId, appId),
            new Claim(UserClaims.JwtId, Guid.NewGuid().ToString())
        };

        permissions.ForEach(permission => claims.Add(new Claim(UserClaims.Permissions, permission)));
        
        roles.ForEach(role => claims.Add(new Claim(UserClaims.Roles, role)));

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