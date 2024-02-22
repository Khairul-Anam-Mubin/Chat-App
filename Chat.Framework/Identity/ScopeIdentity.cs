using Chat.Framework.Extensions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Chat.Framework.Identity;

public class ScopeIdentity : IScopeIdentity
{
    private string? AccessToken { get; set; }
    private List<Claim> Claims { get; set; }
    private UserIdentity? UserIdentity { get; set; }

    private readonly IHttpContextAccessor _httpContextAccessor;

    public ScopeIdentity(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        Claims = new List<Claim>();
    }

    public Claim? GetClaimByType(string type)
    {
        return GetClaims().Where(claim => claim.Type == type).FirstOrDefault();
    }

    public List<Claim> GetClaims()
    {
        return Claims;
    }

    public UserIdentity? GetUser()
    {
        return UserIdentity;
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

        Claims = TokenHelper.GetClaims(GetToken());

        UserIdentity = UserIdentity.Create(Claims);

        return this;
    }
}
