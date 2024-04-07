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
    string GenerateAccessToken(User user, string appId, List<string> roles, List<string> permissions);
    List<Claim> GenerateClaims(UserProfile userProfile, string appId);
    List<Claim> GenerateClaims(User user, string appId, List<string> roles, List<string> permissions);
    IResult CheckForValidRefreshTokenRequest(string jwtToken);
    Token GenerateToken(UserProfile userProfile, string appId);
    Token GenerateToken(User user, UserAccess userAccess, string appId);
    Token GenerateToken(User user, string appId, List<string> roles, List<string> permissions);
    TokenValidationParameters GetTokenValidationParameters(
        bool validateIssuer = true,
        bool validateAudience = true,
        bool validateLifetime = true,
        bool validateIssuerSigningKey = true);
}