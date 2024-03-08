using Chat.Domain.Shared.Queries;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Results;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Domain.Entities;
using Chat.Identity.Domain.Repositories;

namespace Chat.Identity.Application.Consumers;

public class UserProfileQueryConsumer : AQueryConsumer<UserProfileQuery, UserProfileResponse>
{
    private readonly IUserRepository _userRepository;

    public UserProfileQueryConsumer(IUserRepository userRepository, IScopeIdentity scopeIdentity)
        : base(scopeIdentity)
    {
        _userRepository = userRepository;
    }

    protected override async Task<IResult<UserProfileResponse>> OnConsumeAsync(UserProfileQuery query, 
        IMessageContext<UserProfileQuery>? context = null)
    {
        var response = new UserProfileResponse();

        var userModels = new List<UserModel>();

        if (query.UserIds != null && query.UserIds.Any())
        {
            userModels.AddRange(await _userRepository.GetUsersByUserIdsAsync(query.UserIds));
        }

        if (query.Emails != null && query.Emails.Any())
        {
            userModels.AddRange(await _userRepository.GetUsersByEmailsAsync(query.Emails));
        }

        if (context is null)
        {
            foreach (var userModel in userModels)
            {
                response.AddItem(userModel.ToUserProfile());
            }

            return Result.Success(response);
        }

        foreach (var userModel in userModels)
        {
            response.Profiles.Add(userModel.ToUserProfile());
        }

        return Result.Success(response);
    }
}