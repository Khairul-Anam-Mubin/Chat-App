using Chat.Application.Shared.Helpers;
using Chat.Domain.Shared.Constants;
using Chat.Domain.Shared.Entities;
using System.Security.Claims;

namespace Chat.Application.Shared.Providers;

public class IdentityProvider
{
    public static UserProfile GetUserProfile(List<Claim> claims)
    {
        var userProfile = new UserProfile();

        foreach (var claim in claims)
        {
            switch (claim.Type)
            {
                case UserClaims.UserId:
                    userProfile.Id = claim.Value;
                    break;
                case UserClaims.FirstName:
                    userProfile.FirstName = claim.Value;
                    break;
                case UserClaims.LastName:
                    userProfile.LastName = claim.Value;
                    break;
                case UserClaims.UserName:
                    userProfile.UserName = claim.Value;
                    break;
                case UserClaims.Email:
                    userProfile.Email = claim.Value;
                    break;
                case UserClaims.ProfilePictureId:
                    userProfile.ProfilePictureId = claim.Value;
                    break;
                default:
                    if (string.IsNullOrEmpty(userProfile.Email) && IsEmail(claim))
                    {
                        userProfile.Email = claim.Value;   
                    }
                    break;
            }
        }

        return userProfile;
    }

    public static UserProfile GetUserProfile(string? accessToken)
    {
        var claims = TokenHelper.GetClaims(accessToken);

        return GetUserProfile(claims);
    }

    public static UserProfile GetUserProfile(ClaimsPrincipal? claimsPrincipal)
    {
        if (claimsPrincipal is null || claimsPrincipal.Claims is null)
        {
            return new UserProfile();
        }

        return GetUserProfile(claimsPrincipal.Claims.ToList());
    }

    private static bool IsEmail(Claim claim)
    {
        if (claim is null) return false;
        if (claim.Type == ClaimTypes.Email) return true;
        if (claim.Type.Contains("emailaddress"))
        {
            return true;
        }
        return false;
    }
}