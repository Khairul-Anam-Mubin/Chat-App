using Chat.Application.Shared.Helpers;
using Chat.Application.Shared.Providers;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Chat.Identity.Application.Interfaces;
using Chat.Identity.Domain.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Identity.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<RefreshTokenCommand, CommandResponse>), ServiceLifetime.Singleton)]
public class RefreshTokenCommandHandler : ACommandHandler<RefreshTokenCommand>
{
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    protected override async Task<CommandResponse> OnHandleAsync(RefreshTokenCommand command)
    {
        var response = command.CreateResponse();

        if (!TokenHelper.IsTokenValid(command.Token.AccessToken, _tokenService.GetTokenValidationParameters(true, true, false)))
        {
            throw new Exception("Invalid access token");
        }

        if (!TokenHelper.IsExpired(command.Token.AccessToken))
        {
            throw new Exception("AccessToken not expired yet!");
        }
        
        var accessModel = await _tokenService.GetAccessModelByRefreshTokenAsync(command.Token.RefreshToken);
        if (accessModel == null || accessModel.AccessToken != command.Token.AccessToken)
        {
            throw new Exception("Refresh or AccessToken Error");
        }
        
        if (command.AppId != accessModel.AppId)
        {
            throw new Exception("AppId Error");
        }
        
        if (accessModel.Expired)
        {
            await _tokenService.RevokeAllTokensByUserId(accessModel.UserId);
            throw new Exception("Suspicious Token refresh attempt");
        }
        
        accessModel.Expired = true;
        
        await _tokenService.SaveAccessModelAsync(accessModel);
        
        var userProfile = IdentityProvider.GetUserProfile(command.Token.AccessToken);
        
        var token = await _tokenService.CreateTokenAsync(userProfile, command.AppId);
        if (token == null)
        {
            throw new Exception("Token Error");
        }
        
        response.SetData("Token", token);
        
        return response;
    }
}