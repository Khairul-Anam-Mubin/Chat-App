using Chat.Domain.Shared.Queries;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Application.Interfaces;
using Chat.Identity.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Identity.Application.QueryHandlers
{
    [ServiceRegister(typeof(IRequestHandler<UserProfileQuery, QueryResponse>), ServiceLifetime.Singleton)]
    public class UserProfileQueryHandler : AQueryHandler<UserProfileQuery>
    {
        private readonly IUserRepository _userRepository;
        public UserProfileQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected override async Task<QueryResponse> OnHandleAsync(UserProfileQuery query)
        {
            var response = query.CreateResponse();
            var userModels = new List<UserModel>();
            if (query.UserIds != null && query.UserIds.Any())
            {
                userModels.AddRange(await _userRepository.GetUsersByUserIdsAsync(query.UserIds));
            }
            if (query.Emails != null && query.Emails.Any())
            {
                userModels.AddRange(await _userRepository.GetUsersByEmailsAsync(query.Emails));
            }
            foreach (var userModel in userModels)
            {
                response.AddItem(userModel.ToUserProfile());
            }
            return response;
        }
    }
}