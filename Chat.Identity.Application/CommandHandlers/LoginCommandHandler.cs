using Chat.Framework.Mediators;
using Chat.Framework.RequestResponse;
using Chat.Identity.Application.Commands;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Domain.Interfaces;

namespace Chat.Identity.Application.CommandHandlers;

public class LoginCommandHandler : IHandler<LoginCommand, IResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<IResponse> HandleAsync(LoginCommand command)
    {
        var user = await _userRepository.GetUserByEmailAsync(command.Email);

        if (user == null)
        {
            return Response.Error("Email error!!");
        }

        if (user.Password != command.Password)
        {
            return Response.Error("Password error!!");
        }

        var userProfile = user.ToUserProfile();

        var token = await _tokenService.CreateTokenAsync(userProfile, command.AppId);
        if (token == null)
        {
            return Response.Error("Token Creation Failed");
        }

        var response = Response.Success();
        
        response.SetData("Token", token);
        response.Message = "Logged in successfully";

        return response;
    }
}