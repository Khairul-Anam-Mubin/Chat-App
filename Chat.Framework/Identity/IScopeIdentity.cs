using System.Security.Claims;

namespace Chat.Framework.Identity;

///<summary>
/// Call on Transient or scoped services only else it will create captive dependency
///</summary>
public interface IScopeIdentity
{
    /// <summary>
    /// Swith identity in same scope if needed
    /// </summary>
    IScopeIdentity SwitchIdentity(string? accessToken);
    UserIdentity? GetUser();
    string GetUserId();
    string? GetToken();
    List<Claim> GetClaims();
    Claim? GetClaimByType(string type);
    bool HasClaim(string claimType);
}
