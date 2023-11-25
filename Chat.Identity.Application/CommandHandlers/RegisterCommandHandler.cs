using Chat.Framework.Mediators;
using Chat.Framework.RequestResponse;
using Chat.Identity.Application.Commands;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Domain.Interfaces;

namespace Chat.Identity.Application.CommandHandlers;

public class RegisterCommandHandler : IHandler<RegisterCommand, IResponse>
{
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IResponse> HandleAsync(RegisterCommand command)
    {
        if (await _userRepository.IsUserExistAsync(command.UserModel))
        {
            return Response.Error("User email or id already exists!!");
        }
        
        command.UserModel.Id = Guid.NewGuid().ToString();
        command.UserModel.UserName = $"{command.UserModel.FirstName}_{command.UserModel.LastName}";
        
        if (!await _userRepository.SaveAsync(command.UserModel))
        {
            return Response.Error("Some anonymous problem occurred!!");
        }

        var response = Response.Success();
        
        response.Message = "User Created Successfully!!";
        response.SetData("UserProfile", command.UserModel.ToUserProfile());
        
        return response;
    }
}