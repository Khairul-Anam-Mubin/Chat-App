using Chat.Identity.Application.Commands;
using Chat.Identity.Application.Dtos;
using Chat.Identity.Domain.Repositories;
using Peacious.Framework.CQRS;
using Peacious.Framework.Results;

namespace Chat.Identity.Application.CommandHandlers;

public class LoginCommandHandler : ICommandHandler<LoginCommand, TokenDto>
{
    private readonly IUserRepository _userRepository;
    private readonly ICommandService _commandService;

    public LoginCommandHandler(
        IUserRepository userRepository,
        ICommandService commandService)
    {
        _userRepository = userRepository;
        _commandService = commandService;
    }

    public Task<IResult<TokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return HandleAsync(request);
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

        var generateTokenCommand = new GenerateTokenCommand
        {
            UserId = user.Id,
            AppId = command.AppId
        };

        var generateTokenResult = 
            await _commandService.ExecuteAsync<GenerateTokenCommand, TokenDto>(generateTokenCommand);

        var tokenDto = generateTokenResult.Value;

        if (generateTokenResult.IsFailure || tokenDto is null)
        {
            return generateTokenResult;
        }
        
        return Result.Success(tokenDto, "Logged in successfully");
    }
}