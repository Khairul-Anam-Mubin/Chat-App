using System.Security.Claims;
using Chat.Domain.Shared.Entities;
using Chat.Framework.Results;
using Chat.Identity.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Chat.Identity.Domain.Services;

public interface ITokenService
{
    string GenerateAccessToken(List<Claim> claims);
    string GenerateAccessToken(UserProfile userProfile, string appId);
    List<Claim> GenerateClaims(UserProfile userProfile, string appId);
    IResult CheckForValidRefreshTokenRequest(string jwtToken);
    Token GenerateToken(UserProfile userProfile, string appId);
    TokenValidationParameters GetTokenValidationParameters(
        bool validateIssuer = true,
        bool validateAudience = true,
        bool validateLifetime = true,
        bool validateIssuerSigningKey = true);
}