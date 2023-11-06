using Chat.Domain.Shared.Queries;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Extensions;
using Chat.Framework.Mediators;
using Chat.Framework.MessageBrokers;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Domain.Interfaces;
using Chat.Identity.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Identity.Application.Consumers;

[ServiceRegister(typeof(IRequestHandler<UserProfileQuery, UserProfileQueryResponse>), ServiceLifetime.Transient)]
public class UserProfileQueryConsumer : AQueryConsumer<UserProfileQuery, UserProfileQueryResponse>
{
    private readonly IUserRepository _userRepository;

    public UserProfileQueryConsumer(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    protected override async Task<UserProfileQueryResponse> OnConsumeAsync(UserProfileQuery query, 
        IMessageContext<UserProfileQuery>? context = null)
    {
        var response = new UserProfileQueryResponse();

        var userModels = new List<UserModel>();

        if (query.UserIds != null && query.UserIds.Any())
        {
            userModels.AddRange(await _userRepository.GetUsersByUserIdsAsync(query.UserIds));
        }

        if (query.Emails != null && query.Emails.Any())
        {
            userModels.AddRange(await _userRepository.GetUsersByEmailsAsync(query.Emails));
        }

        if (context == null)
        {
            foreach (var userModel in userModels)
            {
                response.AddItem(userModel.ToUserProfile().SmartCast<object>()!);
            }

            return response;
        }

        foreach (var userModel in userModels)
        {
            response.Profiles.Add(userModel.ToUserProfile());
        }

        return response;
    }
}