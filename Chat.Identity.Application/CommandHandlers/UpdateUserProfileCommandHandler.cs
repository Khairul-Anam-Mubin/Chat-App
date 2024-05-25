using Chat.Domain.Shared.Entities;
using Chat.Identity.Application.Commands;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Domain.Repositories;
using Peacious.Framework.CQRS;
using Peacious.Framework.Identity;
using Peacious.Framework.Results;

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

    public Task<IResult<UserProfile>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        return HandleAsync(request);
    }

    public async Task<IResult<UserProfile>> HandleAsync(UpdateUserProfileCommand command)
    {
        var userId = _scopeIdentity.GetUserId()!;
        
        var user = 
            await _userRepository.GetByIdAsync(userId);
        
        if (user is null)
        {
            return Result.Error<UserProfile>("UserProfile not found");
        }

        var firstName = command.FirstName;
        var lastName = command.LastName;
        var birthday = command.BirthDay;
        var about = command.About;
        var profilePictureId = command.ProfilePictureId;

        var updateResult = 
            user.Update(firstName, lastName, birthday, about, profilePictureId);
        
        if (updateResult.IsFailure)
        {
            return Result.Error<UserProfile>(updateResult.Message);
        }

        await _userRepository.SaveAsync(user);

        return Result.Success(user.ToUserProfile());
    }
}