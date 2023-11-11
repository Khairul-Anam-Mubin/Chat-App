using Chat.Framework.Attributes;
using Chat.Framework.Interfaces;
using Chat.Framework.Mediators;
using Chat.Framework.Models;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Domain.Commands;
using Chat.Identity.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Identity.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<LoginCommand, IResponse>), ServiceLifetime.Singleton)]
public class LoginCommandHandler : IRequestHandler<LoginCommand, IResponse>
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
        var response = Response.Success();

        var user = await _userRepository.GetUserByEmailAsync(command.Email);

        if (user == null)
        {
            throw new Exception("Email error!!");
        }

        if (user.Password != command.Password)
        {
            throw new Exception("Password error!!");
        }

        var userProfile = user.ToUserProfile();

        var token = await _tokenService.CreateTokenAsync(userProfile, command.AppId);
        if (token == null)
        {
            throw new Exception("Token Creation Failed");
        }

        response.SetData("Token", token);
        response.Message = "Logged in successfully";

        return response;
    }
}