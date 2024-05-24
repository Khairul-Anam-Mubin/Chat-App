using Chat.Identity.Application.Commands;
using Chat.Identity.Domain.Entities;
using Chat.Identity.Domain.Repositories;
using Chat.Identity.Domain.Services;
using Peacious.Framework.CQRS;
using Peacious.Framework.Identity;
using Peacious.Framework.Results;

namespace Chat.Identity.Application.CommandHandlers;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IScopeIdentity _scopeIdentity;

    public RegisterCommandHandler(IUserRepository userRepository, ITokenService tokenService, IScopeIdentity scopeIdentity)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _scopeIdentity = scopeIdentity;
    }

    public async Task<IResult> HandleAsync(RegisterCommand command)
    {
        var userCreatedResult = 
            User.Create(
                command.FirstName, 
                command.LastName, 
                command.BirthDay, 
                command.Email, 
                command.Password,
                await _userRepository.IsUserExistAsync(command.Email));

        var user = userCreatedResult.Value;

        if (userCreatedResult.IsFailure || user is null)
        {
            return userCreatedResult;
        }

        var accessToken = 
            _tokenService.GenerateAccessToken(user, Guid.NewGuid().ToString(), new List<string>(), new List<string>());

        _scopeIdentity.SwitchIdentity(accessToken);

        if (!await _userRepository.SaveAsync(user))
        {
            return Error.SaveProblem<User>();
        }

        return Result.Success("UserProfile Created Successfully!!");
    }
}