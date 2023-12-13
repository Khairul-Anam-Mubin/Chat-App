using Chat.Framework.CQRS;
using Chat.Framework.Results;
using Chat.Identity.Application.Commands;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Domain.Interfaces;
using Chat.Identity.Domain.Models;

namespace Chat.Identity.Application.CommandHandlers;

public class LoginCommandHandler : ICommandHandler<LoginCommand, Token>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<IResult<Token>> HandleAsync(LoginCommand command)
    {
        var user = await _userRepository.GetUserByEmailAsync(command.Email);

        if (user is null)
        {
            return Result<Token>.Error("Email error!!");
        }

        if (user.Password != command.Password)
        {
            return Result<Token>.Error("Password error!!");
        }

        if (!user.IsEmailVerified)
        {
            // Todo: will send email for verification
            return Result<Token>.Error("Plz verify your email.Check your inbox!!");
        }

        var userProfile = user.ToUserProfile();

        var token = await _tokenService.CreateTokenAsync(userProfile, command.AppId);
        
        if (token is null)
        {
            return Result<Token>.Error("Token Creation Failed");
        }

        return Result<Token>.Success(token, "Logged in successfully"); ;
    }
}