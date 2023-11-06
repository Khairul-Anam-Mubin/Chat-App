using Chat.Application.Shared.Helpers;
using Chat.Application.Shared.Providers;
using Chat.Framework.Attributes;
using Chat.Framework.Extensions;
using Chat.Identity.Domain.Interfaces;

namespace Chat.Api.Middlewares;

[ServiceRegister(typeof(LastSeenMiddleware), ServiceLifetime.Transient)]
public class LastSeenMiddleware : IMiddleware
{
    private readonly ITokenService _tokenService;

    public LastSeenMiddleware(ITokenService tokenService)
    {
        _tokenService = tokenService;
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

                Console.WriteLine("Last seen activity tracing from LastSeenMiddleware\n");
            }
        }

        await next(context);

        Console.WriteLine("Returning from LastSeenMiddleware\n");
    }
}