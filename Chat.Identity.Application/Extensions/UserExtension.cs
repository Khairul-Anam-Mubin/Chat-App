using Chat.Domain.Shared.Entities;
using Chat.Identity.Domain.Entities;

namespace Chat.Identity.Application.Extensions;

public static class UserExtension
{
    public static UserProfile ToUserProfile(this User user)
    {
        var userProfile = new UserProfile
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            BirthDay = user.BirthDay,
            About = user.About,
            ProfilePictureId = user.ProfilePictureId,
            Email = user.Email
        };
        return userProfile;
    }
}