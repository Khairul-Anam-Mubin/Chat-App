using System.Security.Claims;
using System.Text;
using Chat.Application.Shared.Helpers;
using Chat.Domain.Shared.Constants;
using Chat.Domain.Shared.Models;
using Chat.Framework.Attributes;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Application.Interfaces;
using Chat.Identity.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Chat.Identity.Application.Services;

[ServiceRegister(typeof(ITokenService), ServiceLifetime.Singleton)]
public class TokenService : ITokenService
{
    private readonly TokenConfig _tokenConfig;
    private readonly IAccessRepository _accessRepository;

    public TokenService(IAccessRepository accessRepository, IConfiguration configuration)
    {
        _tokenConfig = configuration.GetSection("TokenConfig").Get<TokenConfig>();
        _accessRepository = accessRepository;
    }

    public async Task<Token?> CreateTokenAsync(UserProfile userProfile, string appId)
    {
        var accessModel = GenerateAccessModel(userProfile, appId);
        var isSave = await _accessRepository.SaveAsync(accessModel);
        if (isSave == false) return null;
        return accessModel.ToToken();
    }

    public async Task<bool> RevokeAllTokenByAppIdAsync(string appId)
    {
        return await _accessRepository.DeleteAllTokenByAppId(appId);
    }

    public async Task<bool> RevokeAllTokensByUserId(string userId)
    {
        return await _accessRepository.DeleteAllTokensByUserId(userId);
    }

    public async Task<bool> SaveAccessModelAsync(AccessModel accessModel)
    {
        return await _accessRepository.SaveAsync(accessModel);
    }
    public async Task<AccessModel?> GetAccessModelByRefreshTokenAsync(string refreshToken)
    {
        return await _accessRepository.GetByIdAsync(refreshToken);
    }

    public AccessModel GenerateAccessModel(UserProfile userProfile, string appId)
    {
        var claims = GenerateClaims(userProfile, appId);
        var accessToken = TokenHelper.GenerateJwtToken(
            _tokenConfig.SecretKey, 
            _tokenConfig.Issuer, 
            _tokenConfig.Audience, 
            _tokenConfig.ExpirationTimeInSec, 
            claims);
        var refreshToken = TokenHelper.GenerateRefreshToken();
        var accessModel = new AccessModel
        {
            Id = refreshToken,
            AccessToken = accessToken,
            AppId = appId,
            UserId = userProfile.Id,
            Expired = false,
            CreatedAt = DateTime.UtcNow
        };
        return accessModel;
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