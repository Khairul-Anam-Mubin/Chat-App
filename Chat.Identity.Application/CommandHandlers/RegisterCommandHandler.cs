using Chat.Framework.Attributes;
using Chat.Framework.Mediators;
using Chat.Framework.Models;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Application.Interfaces;
using Chat.Identity.Domain.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Identity.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<RegisterCommand, Response>), ServiceLifetime.Singleton)]
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Response>
{
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Response> HandleAsync(RegisterCommand command)
    {
        var response = command.CreateResponse();

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
        
        return (Response)response;
    }
}