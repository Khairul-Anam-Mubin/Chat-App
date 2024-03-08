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
        var validationCheckResult = 
            _tokenService.CheckForValidRefreshTokenRequest(command.AccessToken);

        if (validationCheckResult.IsFailure)
        {
            return (IResult<Token>)validationCheckResult;
        }

        var accessModel = 
            await _tokenService.GetAccessModelByRefreshTokenAsync(command.RefreshToken);

        if (accessModel is null)
        {
            return Result.Error<Token>("AccessModel not found");
        }

        var allowedResult = 
            accessModel.IsTokenAllowedToRefresh(command.AccessToken, command.AppId);

        if (allowedResult.IsFailure)
        {
            return (IResult<Token>)allowedResult;
        }

        var validRefreshAttemptResult = accessModel.CheckForValidRefreshAttempt();

        if (validRefreshAttemptResult.IsFailure)
        {
            await _tokenService.RevokeAllTokensByUserId(accessModel.UserId);

            return (IResult<Token>)validRefreshAttemptResult;
        }

        accessModel.MakeTokenExpired();
        
        await _tokenService.SaveAccessModelAsync(accessModel);
        
        var userProfile = IdentityProvider.GetUserProfile(command.AccessToken);
        
        var token = await _tokenService.CreateTokenAsync(userProfile, command.AppId);

        if (token is null)
        {
            return Result.Error<Token>("Token Creation Failed");
        }

        return Result.Success(token);
    }
} 