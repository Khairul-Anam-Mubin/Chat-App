using Chat.Framework.CQRS;
using Chat.Framework.Results;
using Chat.Identity.Application.Commands;
using Chat.Identity.Domain.Entities;
using Chat.Identity.Domain.Repositories;

namespace Chat.Identity.Application.CommandHandlers;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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

        if (!await _userRepository.SaveAsync(user))
        {
            return Error.SaveProblem<User>();
        }

        return Result.Success("UserProfile Created Successfully!!");
    }
}