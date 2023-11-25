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
        if (!TokenHelper.IsTokenValid(command.Token.AccessToken, _tokenService.GetTokenValidationParameters(true, true, false)))
        {
            return Response.Error("Invalid access token");
        }

        if (!TokenHelper.IsExpired(command.Token.AccessToken))
        {
            return Response.Error("AccessToken not expired yet!");
        }
        
        var accessModel = await _tokenService.GetAccessModelByRefreshTokenAsync(command.Token.RefreshToken);
        if (accessModel == null || accessModel.AccessToken != command.Token.AccessToken)
        {
            return Response.Error("Refresh or AccessToken ErrorMessage");
        }
        
        if (command.AppId != accessModel.AppId)
        {
            return Response.Error("AppId ErrorMessage");
        }
        
        if (accessModel.Expired)
        {
            await _tokenService.RevokeAllTokensByUserId(accessModel.UserId);
            return Response.Error("Suspicious Token refresh attempt");
        }
        
        accessModel.Expired = true;
        
        await _tokenService.SaveAccessModelAsync(accessModel);
        
        var userProfile = IdentityProvider.GetUserProfile(command.Token.AccessToken);
        
        var token = await _tokenService.CreateTokenAsync(userProfile, command.AppId);
        if (token == null)
        {
            return Response.Error("Token ErrorMessage");
        }

        var response = Response.Success();

        response.SetData("Token", token);
        
        return response;
    }
}