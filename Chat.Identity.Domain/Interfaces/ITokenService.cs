using System.Security.Claims;
using Chat.Domain.Shared.Entities;
using Chat.Framework.Results;
using Chat.Identity.Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace Chat.Identity.Domain.Interfaces;

public interface ITokenService
{
    IResult CheckForValidRefreshTokenRequest(string jwtToken);
    AccessModel GenerateAccessModel(UserProfile userProfile, string appId);
    List<Claim> GenerateClaims(UserProfile userProfile, string appId);
    TokenValidationParameters GetTokenValidationParameters(bool validateIssuer = true, bool validateAudience = true, bool validateLifetime = true, bool validateIssuerSigningKey = true);
    Task<Token?> CreateTokenAsync(UserProfile userProfile, string appId);
    Task<bool> SaveAccessModelAsync(AccessModel accessModel);
    Task<bool> RevokeAllTokenByAppIdAsync(string appId);
    Task<bool> RevokeAllTokensByUserId(string userId);
    Task<AccessModel?> GetAccessModelByRefreshTokenAsync(string refreshToken);

}