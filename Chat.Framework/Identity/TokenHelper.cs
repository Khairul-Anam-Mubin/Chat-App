using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Chat.Framework.Identity;

public static class TokenHelper
{
    public static string GenerateJwtToken(
        string issuer,
        string audience,
        string secretKey,
        int expiredTimeInSec,
        List<Claim>? claims = null,
        string securityAlgorithm = "")
    {
        if (string.IsNullOrEmpty(securityAlgorithm))
        {
            securityAlgorithm = SecurityAlgorithms.HmacSha256;
        }
        try
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, securityAlgorithm);
            var expiredTime = DateTime.Now.AddSeconds(expiredTimeInSec);
            var tokenOptions = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiredTime,
                signingCredentials: signingCredentials
            );
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return token;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    public static string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }

    public static List<Claim> GetClaims(string? accessToken)
    {
        try
        {
            accessToken = GetPreparedToken(accessToken);
            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            return jwtSecurityToken.Claims.ToList();
        }
        catch (Exception)
        {
            return new List<Claim>();
        }
    }

    public static bool TryValidateToken(string? accessToken, TokenValidationParameters validationParameters, out string validationMessage)
    {
        try
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("AccessToken not found");
            }

            accessToken = GetPreparedToken(accessToken);
            new JwtSecurityTokenHandler()
                .ValidateToken(accessToken, validationParameters, out var validatedToken);

            validationMessage = "Successfully token validated with the given parameters";
            
            return true;
        }
        catch (Exception ex)
        {
            validationMessage = ex.Message;
            return false;
        }
    }

    public static bool IsExpired(string? accessToken)
    {
        try
        {
            accessToken = GetPreparedToken(accessToken);
            var securityToken = new JwtSecurityToken(accessToken);
            bool isExpired = securityToken.ValidTo < DateTime.UtcNow;
            return isExpired;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static string? GetPreparedToken(string? accessToken)
    {
        if (string.IsNullOrEmpty(accessToken) == false && accessToken.StartsWith("Bearer "))
        {
            return accessToken.Replace("Bearer ", "");
        }
        return accessToken;
    }
}