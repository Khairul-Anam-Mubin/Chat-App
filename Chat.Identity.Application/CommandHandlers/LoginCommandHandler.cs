using Chat.Framework.CQRS;
using Chat.Framework.Results;
using Chat.Identity.Application.Commands;
using Chat.Identity.Application.Dtos;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Domain.Repositories;
using Chat.Identity.Domain.Services;

namespace Chat.Identity.Application.CommandHandlers;

public class LoginCommandHandler : ICommandHandler<LoginCommand, Token>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IAccessRepository _accessRepository;

    public LoginCommandHandler(IUserRepository userRepository, ITokenService tokenService, IAccessRepository accessRepository)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _accessRepository = accessRepository;
    }

    public async Task<IResult<Token>> HandleAsync(LoginCommand command)
    {
        var user = await _userRepository.GetUserByEmailAsync(command.Email);

        if (user is null)
        {
            return Result.Error<Token>("Email error!!");
        }

        var loginResult = user.LogIn(command.Password);

        if (loginResult.IsFailure)
        {
            return Result.Error<Token>(loginResult.Message);
        }

        var userProfile = user.ToUserProfile();

        var accessModel = _tokenService.GenerateAccessModel(userProfile, command.AppId);

        await _accessRepository.SaveAsync(accessModel);

        var token = accessModel.ToToken();
        
        if (token is null)
        {
            return Result.Error<Token>("Token Creation Failed");
        }

        return Result.Success(token, "Logged in successfully"); ;
    }
}