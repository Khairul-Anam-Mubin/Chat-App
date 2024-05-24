using Chat.Domain.Shared.Queries;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Domain.Entities;
using Chat.Identity.Domain.Repositories;
using Peacious.Framework.CQRS;
using Peacious.Framework.Identity;
using Peacious.Framework.MessageBrokers;
using Peacious.Framework.Results;

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

        var users = new List<User>();

        if (query.UserIds != null && query.UserIds.Any())
        {
            users.AddRange(await _userRepository.GetUsersByUserIdsAsync(query.UserIds));
        }

        if (query.Emails != null && query.Emails.Any())
        {
            users.AddRange(await _userRepository.GetUsersByEmailsAsync(query.Emails));
        }

        if (context is null)
        {
            foreach (var user in users)
            {
                response.AddItem(user.ToUserProfile());
            }

            return Result.Success(response);
        }

        foreach (var user in users)
        {
            response.Profiles.Add(user.ToUserProfile());
        }

        return Result.Success(response);
    }
}