using Chat.Framework.Attributes;
using Chat.Framework.Interfaces;
using Chat.Framework.Mediators;
using Chat.Framework.Models;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Domain.Commands;
using Chat.Identity.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Identity.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<RegisterCommand, IResponse>), ServiceLifetime.Singleton)]
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, IResponse>
{
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IResponse> HandleAsync(RegisterCommand command)
    {
        var response = Response.Success();

        if (await _userRepository.IsUserExistAsync(command.UserModel))
        {
            throw new Exception("User email or id already exists!!");
        }
        
        command.UserModel.Id = Guid.NewGuid().ToString();
        command.UserModel.UserName = $"{command.UserModel.FirstName}_{command.UserModel.LastName}";
        
        if (!await _userRepository.SaveAsync(command.UserModel))
        {
            throw new Exception("Some anonymous problem occured!!");
        }
        
        response.Message = "User Created Successfully!!";
        response.SetData("UserProfile", command.UserModel.ToUserProfile());
        
        return response;
    }
}