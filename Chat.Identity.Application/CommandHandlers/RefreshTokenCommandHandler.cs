using Chat.Application.Shared.Helpers;
using Chat.Application.Shared.Providers;
using Chat.Framework.CQRS;
using Chat.Framework.Results;
using Chat.Identity.Application.Commands;
using Chat.Identity.Domain.Interfaces;

namespace Chat.Identity.Application.CommandHandlers;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand>
{
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<IResult> HandleAsync(RefreshTokenCommand command)
    {
        if (!TokenHelper.IsTokenValid(command.Token.AccessToken, _tokenService.GetTokenValidationParameters(true, true, false)))
        {
            return Result.Error("Invalid access token");
        }

        if (!TokenHelper.IsExpired(command.Token.AccessToken))
        {
            return Result.Error("AccessToken not expired yet!");
        }
        
        var accessModel = await _tokenService.GetAccessModelByRefreshTokenAsync(command.Token.RefreshToken);
        if (accessModel == null || accessModel.AccessToken != command.Token.AccessToken)
        {
            return Result.Error("Refresh or AccessToken ErrorMessage");
        }
        
        if (command.AppId != accessModel.AppId)
        {
            return Result.Error("AppId ErrorMessage");
        }
        
        if (accessModel.Expired)
        {
            await _tokenService.RevokeAllTokensByUserId(accessModel.UserId);
            return Result.Error("Suspicious Token refresh attempt");
        }
        
        accessModel.Expired = true;
        
        await _tokenService.SaveAccessModelAsync(accessModel);
        
        var userProfile = IdentityProvider.GetUserProfile(command.Token.AccessToken);
        
        var token = await _tokenService.CreateTokenAsync(userProfile, command.AppId);
        if (token == null)
        {
            return Result.Error("Token ErrorMessage");
        }

        var response = Result.Success();

        response.SetData("Token", token);
        
        return response;
    }
}