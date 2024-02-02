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
        return GetClaims().Where(claim => claim.Type == type).FirstOrDefault();
    }

    public List<Claim> GetClaims()
    {
        return TokenHelper.GetClaims(GetToken());
    }

    public UserIdentity? GetUser()
    {
        var claims = GetClaims();

        var userIdentity = new UserIdentity();

        foreach (var claim in claims)
        {
            if (claim.Type == "user_name")
            {
                userIdentity.Name = claim.Value;
            }
            else if (claim.Type == "user_id")
            {
                userIdentity.Id = claim.Value;
            }
            else if (claim.Type == "email")
            {
                userIdentity.Email = claim.Value;
            }
        }

        return userIdentity;
    }

    public string? GetToken()
    {
        if (!string.IsNullOrEmpty(AccessToken))
        {
            return AccessToken;
        }

        return _httpContextAccessor.HttpContext?.GetAccessToken();
    }

    public string? GetUserId()
    {
        return GetUser()?.Id;
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
