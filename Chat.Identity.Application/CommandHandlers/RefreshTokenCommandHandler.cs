using Chat.Application.Shared.Providers;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
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
            return Result.Error<Token>("Invalid access token");
        }

        if (!TokenHelper.IsExpired(command.Token.AccessToken))
        {
            return Result.Error<Token>("AccessToken not expired yet!");
        }
        
        var accessModel = 
            await _tokenService.GetAccessModelByRefreshTokenAsync(command.Token.RefreshToken);

        if (accessModel is null || accessModel.AccessToken != command.Token.AccessToken)
        {
            return Result.Error<Token>("Refresh or AccessToken Error");
        }
        
        if (command.AppId != accessModel.AppId)
        {
            return Result.Error<Token>("AppId Error");
        }
        
        if (accessModel.Expired)
        {
            await _tokenService.RevokeAllTokensByUserId(accessModel.UserId);
            // Todo: will send email for suspicious attempt
            return Result.Error<Token>("Suspicious Token refresh attempt");
        }
        
        accessModel.Expired = true;
        
        await _tokenService.SaveAccessModelAsync(accessModel);
        
        var userProfile = IdentityProvider.GetUserProfile(command.Token.AccessToken);
        
        var token = await _tokenService.CreateTokenAsync(userProfile, command.AppId);

        if (token is null)
        {
            return Result.Error<Token>("Token Creation Failed");
        }

        return Result.Success(token);
    }
}