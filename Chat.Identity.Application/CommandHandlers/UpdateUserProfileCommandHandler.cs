using Chat.Framework.Attributes;
using Chat.Framework.Mediators;
using Chat.Framework.RequestResponse;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Domain.Commands;
using Chat.Identity.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Identity.Application.CommandHandlers;

[ServiceRegister(typeof(IHandler<UpdateUserProfileCommand, IResponse>), ServiceLifetime.Singleton)]
public class UpdateUserProfileCommandHandler : 
    IHandler<UpdateUserProfileCommand, IResponse>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserProfileCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IResponse> HandleAsync(UpdateUserProfileCommand command)
    {
        var response = Response.Success();

        var requestUpdateModel = command.UserModel;
        
        var userModel = await _userRepository.GetUserByEmailAsync(requestUpdateModel.Email);
        if (userModel == null)
        {
            throw new Exception("UserModel not found");
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
        
        response.Message = "User Updated Successfully!!";
        response.SetData("UserProfile", userModel.ToUserProfile());
        
        return response;
    }
}