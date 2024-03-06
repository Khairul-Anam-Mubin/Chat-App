using Chat.Framework.Extensions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Chat.Framework.Identity;

public class ScopeIdentity : IScopeIdentity
{
    private string? AccessToken { get; set; }

    private readonly IHttpContextAccessor _httpContextAccessor;

    public ScopeIdentity(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Claim? GetClaimByType(string type)
    {
        return GetClaims().FirstOrDefault(claim => claim.Type == type);
    }

    public List<Claim> GetClaims()
    {
        return TokenHelper.GetClaims(GetToken());
    }

    public UserIdentity? GetUser()
    {
        return UserIdentity.Create(GetClaims());
    }

    public string? GetToken()
    {
        if (!string.IsNullOrEmpty(AccessToken))
        {
            return AccessToken;
        }

        return _httpContextAccessor.HttpContext?.GetAccessToken();
    }

    public string GetUserId()
    {
        return GetUser()?.Id ?? string.Empty;
    }

    public bool HasClaim(string claimType)
    {
        return GetClaims().Count(x => x.Type == claimType) > 0;
    }

    public IScopeIdentity SwitchIdentity(string? accessToken)
    {
        AccessToken = accessToken;

        return this;
    }
}
