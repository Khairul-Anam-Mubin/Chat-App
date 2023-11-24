using Chat.Application.Shared.Helpers;
using Chat.Application.Shared.Providers;
using Chat.Framework.Mediators;
using Chat.Framework.RequestResponse;
using Chat.Identity.Application.Commands;
using Chat.Identity.Domain.Interfaces;

namespace Chat.Identity.Application.CommandHandlers;

public class RefreshTokenCommandHandler : IHandler<RefreshTokenCommand, IResponse>
{
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<IResponse> HandleAsync(RefreshTokenCommand command)
    {
        var response = Response.Success();

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
            throw new Exception("Refresh or AccessToken ErrorMessage");
        }
        
        if (command.AppId != accessModel.AppId)
        {
            throw new Exception("AppId ErrorMessage");
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
            throw new Exception("Token ErrorMessage");
        }
        
        response.SetData("Token", token);
        
        return response;
    }
}