using Chat.Domain.Shared.Entities;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.Results;
using Chat.Identity.Application.Commands;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Domain.Repositories;

namespace Chat.Identity.Application.CommandHandlers;

public class UpdateUserProfileCommandHandler : 
    ICommandHandler<UpdateUserProfileCommand, UserProfile>
{
    private readonly IUserRepository _userRepository;
    private readonly IScopeIdentity _scopeIdentity;

    public UpdateUserProfileCommandHandler(IUserRepository userRepository, IScopeIdentity scopeIdentity)
    {
        _userRepository = userRepository;
        _scopeIdentity = scopeIdentity;
    }

    public async Task<IResult<UserProfile>> HandleAsync(UpdateUserProfileCommand command)
    {
        var updateRequestUser = command.UserModel;

        var userId = _scopeIdentity.GetUserId()!;
        
        var user = 
            await _userRepository.GetByIdAsync(userId);
        
        if (user is null)
        {
            return Result.Error<UserProfile>("User not found");
        }

        var updateResult = user.Update(updateRequestUser);
        
        if (updateResult.IsFailure || updateResult.Value is null)
        {
            return Result.Error<UserProfile>(updateResult.Message);
        }

        await _userRepository.SaveAsync(user);

        return Result.Success(user.ToUserProfile());
    }
}