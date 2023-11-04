using Chat.Domain.Shared.Entities;
using Chat.Identity.Domain.Models;

namespace Chat.Identity.Application.Extensions;

public static class UserModelExtension
{
    public static UserProfile ToUserProfile(this UserModel userModel)
    {
        var userProfile = new UserProfile
        {
            Id = userModel.Id,
            FirstName = userModel.FirstName,
            LastName = userModel.LastName,
            UserName = userModel.UserName,
            BirthDay = userModel.BirthDay,
            About = userModel.About,
            ProfilePictureId = userModel.ProfilePictureId,
            Email = userModel.Email,
            PublicKey = userModel.PublicKey
        };
        return userProfile;
    }
}