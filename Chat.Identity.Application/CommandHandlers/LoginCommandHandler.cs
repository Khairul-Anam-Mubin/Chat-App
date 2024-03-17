using Chat.Framework.CQRS;
using Chat.Framework.Results;
using Chat.Identity.Application.Commands;
using Chat.Identity.Application.Dtos;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Domain.Repositories;
using Chat.Identity.Domain.Services;

namespace Chat.Identity.Application.CommandHandlers;

public class LoginCommandHandler : ICommandHandler<LoginCommand, TokenDto>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly ITokenRepository _accessRepository;

    public LoginCommandHandler(IUserRepository userRepository, ITokenService tokenService, ITokenRepository accessRepository)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _accessRepository = accessRepository;
    }

    public async Task<IResult<TokenDto>> HandleAsync(LoginCommand command)
    {
        var user = await _userRepository.GetUserByEmailAsync(command.Email);

        if (user is null)
        {
            return Result.Error<TokenDto>("Email error!!");
        }

        var loginResult = user.LogIn(command.Password);

        if (loginResult.IsFailure)
        {
            return Result.Error<TokenDto>(loginResult.Message);
        }

        var userProfile = user.ToUserProfile();

        var token = _tokenService.GenerateToken(userProfile, command.AppId);

        await _accessRepository.SaveAsync(token);

        var tokenDto = token.ToTokenDto();
        
        if (tokenDto is null)
        {
            return Result.Error<TokenDto>("Token Creation Failed");
        }

        return Result.Success(tokenDto, "Logged in successfully"); ;
    }
}