using Chat.Domain.Shared.Entities;
using Chat.Framework.CQRS;
using Chat.Framework.Results;
using Chat.Identity.Application.Commands;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Domain.Interfaces;

namespace Chat.Identity.Application.CommandHandlers;

public class UpdateUserProfileCommandHandler : 
    ICommandHandler<UpdateUserProfileCommand, UserProfile>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserProfileCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IResult<UserProfile>> HandleAsync(UpdateUserProfileCommand command)
    {
        var requestUpdateModel = command.UserModel;
        
        var userModel = 
            await _userRepository.GetUserByEmailAsync(requestUpdateModel.Email);
        
        if (userModel is null)
        {
            return Result.Error<UserProfile>("UserModel not found");
        }
        
        var updateInfoCount = 0;
        
        if (!string.IsNullOrEmpty(requestUpdateModel.FirstName))
        {
            userModel.FirstName = requestUpdateModel.FirstName;
            updateInfoCount++;
        }
        
        if (!string.IsNullOrEmpty(requestUpdateModel.LastName))
        {
            userModel.LastName = requestUpdateModel.LastName;
            updateInfoCount++;
        }
        
        if (requestUpdateModel.BirthDay != null)
        {
            userModel.BirthDay = requestUpdateModel.BirthDay;
            updateInfoCount++;
        }
        
        if (!string.IsNullOrEmpty(requestUpdateModel.About))
        {
            userModel.About = requestUpdateModel.About;
            updateInfoCount++;
        }
        
        if (!string.IsNullOrEmpty(requestUpdateModel.ProfilePictureId))
        {
            userModel.ProfilePictureId = requestUpdateModel.ProfilePictureId;
            updateInfoCount++;
        }
        
        if (updateInfoCount > 0)
        {
            await _userRepository.SaveAsync(userModel);
        }

        return Result.Success<UserProfile>(userModel.ToUserProfile());
    }
}