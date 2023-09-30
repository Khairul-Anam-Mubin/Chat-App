using Chat.Application.Shared.Helpers;
using Chat.Application.Shared.Providers;
using Chat.Domain.Shared.Commands;
using Chat.Domain.Shared.Models;
using Chat.Framework.Attributes;
using Chat.Framework.Extensions;
using Chat.Framework.Proxy;
using Chat.Identity.Application.Interfaces;

namespace Chat.Api.Middlewares
{
    [ServiceRegister(typeof(LastSeenMiddleware), ServiceLifetime.Transient)]
    public class LastSeenMiddleware : IMiddleware
    {
        private readonly ITokenService _tokenService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICommandQueryProxy _commandQueryProxy;

        public LastSeenMiddleware(ITokenService tokenService, IHttpClientFactory clientFactory, ICommandQueryProxy commandQueryProxy)
        {
            _tokenService = tokenService;
            _httpClientFactory = clientFactory;
            _commandQueryProxy = commandQueryProxy;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Console.WriteLine("Executing LastSeenMiddleware\n");
            var accessToken = context.GetAccessToken();

            if (string.IsNullOrEmpty(accessToken))
            {
                Console.WriteLine("AccessToken not found at LastSeenMiddleware\n");
            }
            else
            {
                Console.WriteLine($"Access Token found : {accessToken}\n");

                if (TokenHelper.IsTokenValid(accessToken, _tokenService.GetTokenValidationParameters()))
                {
                    var userProfile = IdentityProvider.GetUserProfile(accessToken);

                    // Todo: its currently synchronous. have to use message broker for async 
                    // await TrackLastSeenActivityAsync(userProfile, accessToken);

                    Console.WriteLine("Last seen activity tracing from LastSeenMiddleware\n");
                }

            }

            await next(context);
            Console.WriteLine("Returning from LastSeenMiddleware\n");
        }

        private async Task TrackLastSeenActivityAsync(UserProfile userProfile, string accessToken)
        {
            var updateLastSeenCommand = new UpdateLastSeenCommand()
            {
                UserId = userProfile.Id,
                ApiUrl = "https://localhost:50502/api/Activity/track",
                IsActive = true
            };
            updateLastSeenCommand.FireAndForget = true;
            await _commandQueryProxy.GetCommandResponseAsync(updateLastSeenCommand);
        }
    }
}