using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Application.Interfaces;
using Chat.Identity.Domain.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Identity.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<UpdateUserProfileCommand, CommandResponse>), ServiceLifetime.Singleton)]
public class UpdateUserProfileCommandHandler : ICommandHandler<UpdateUserProfileCommand, CommandResponse>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserProfileCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<CommandResponse> HandleAsync(UpdateUserProfileCommand command)
    {
        var response = command.CreateResponse();

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