using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.Results;
using Chat.Identity.Application.Commands;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Domain.Entities;
using Chat.Identity.Domain.Repositories;
using Chat.Identity.Domain.Services;

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
            _tokenService.GenerateAccessToken(user.ToUserProfile(), Guid.NewGuid().ToString());

        _scopeIdentity.SwitchIdentity(accessToken);

        if (!await _userRepository.SaveAsync(user))
        {
            return Error.SaveProblem<User>();
        }

        return Result.Success("UserProfile Created Successfully!!");
    }
}