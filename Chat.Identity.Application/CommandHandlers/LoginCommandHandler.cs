using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Chat.Framework.Results;
using Chat.Identity.Application.Commands;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Domain.Interfaces;

namespace Chat.Identity.Application.CommandHandlers;

public class LoginCommandHandler : ICommandHandler<LoginCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IMediator _mediator;
    private readonly ICommandExecutor _commandExecutor;
    public LoginCommandHandler(IUserRepository userRepository, ITokenService tokenService, IMediator mediator, ICommandExecutor commandExecutor)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _mediator = mediator;
        _commandExecutor = commandExecutor;
    }

    public async Task<IResult> HandleAsync(LoginCommand command)
    {

        var testCommand = new TestCommand();
        var res = await _commandExecutor.ExecuteAsync<TestCommand, TestCommandResponse>(testCommand);

        var user = await _userRepository.GetUserByEmailAsync(command.Email);

        if (user == null)
        {
            return Result.Error("Email error!!");
        }

        if (user.Password != command.Password)
        {
            return Result.Error("Password error!!");
        }

        if (!user.IsEmailVerified)
        {
            return Result.Error("Plz verify your email.!!");
        }

        var userProfile = user.ToUserProfile();

        var token = await _tokenService.CreateTokenAsync(userProfile, command.AppId);
        if (token == null)
        {
            return Result.Error("Token Creation Failed");
        }

        var response = Result.Success();
        
        response.SetData("Token", token);
        response.Message = "Logged in successfully";

        return response;
    }
}