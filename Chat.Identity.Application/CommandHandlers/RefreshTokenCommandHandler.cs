using Chat.Application.Shared.Helpers;
using Chat.Application.Shared.Providers;
using Chat.Framework.CQRS;
using Chat.Framework.Results;
using Chat.Identity.Application.Commands;
using Chat.Identity.Domain.Interfaces;
using Chat.Identity.Domain.Models;

namespace Chat.Identity.Application.CommandHandlers;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, Token>
{
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<IResult<Token>> HandleAsync(RefreshTokenCommand command)
    {
        if (!TokenHelper.IsTokenValid(
                command.Token.AccessToken, 
                _tokenService.GetTokenValidationParameters(
                    true, 
                    true, 
                    false)))
        {
            return Result<Token>.Error("Invalid access token");
        }

        if (!TokenHelper.IsExpired(command.Token.AccessToken))
        {
            return Result<Token>.Error("AccessToken not expired yet!");
        }
        
        var accessModel = 
            await _tokenService.GetAccessModelByRefreshTokenAsync(command.Token.RefreshToken);

        if (accessModel is null || accessModel.AccessToken != command.Token.AccessToken)
        {
            return Result<Token>.Error("Refresh or AccessToken Error");
        }
        
        if (command.AppId != accessModel.AppId)
        {
            return Result<Token>.Error("AppId Error");
        }
        
        if (accessModel.Expired)
        {
            await _tokenService.RevokeAllTokensByUserId(accessModel.UserId);
            // Todo: will send email for suspicious attempt
            return Result<Token>.Error("Suspicious Token refresh attempt");
        }
        
        accessModel.Expired = true;
        
        await _tokenService.SaveAccessModelAsync(accessModel);
        
        var userProfile = IdentityProvider.GetUserProfile(command.Token.AccessToken);
        
        var token = await _tokenService.CreateTokenAsync(userProfile, command.AppId);

        if (token is null)
        {
            return Result<Token>.Error("Token Creation Failed");
        }

        return Result<Token>.Success(token);
    }
}