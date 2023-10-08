using Chat.Application.Shared.Helpers;
using Chat.Domain.Shared.Constants;
using Chat.Domain.Shared.Models;

namespace Chat.Application.Shared.Providers;

public class IdentityProvider
{
    public static UserProfile GetUserProfile(string? accessToken)
    {
        var claims = TokenHelper.GetClaims(accessToken);
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
            }
        }
        return userProfile;
    }
}