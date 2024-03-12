using Chat.Application.Shared.Providers;
using Chat.Framework.CQRS;
using Chat.Framework.Results;
using Chat.Identity.Application.Commands;
using Chat.Identity.Application.Dtos;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Domain.Repositories;
using Chat.Identity.Domain.Services;

namespace Chat.Identity.Application.CommandHandlers;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, Token>
{
    private readonly ITokenService _tokenService;
    private readonly IAccessRepository _accessRepository;

    public RefreshTokenCommandHandler(ITokenService tokenService, IAccessRepository accessRepository)
    {
        _tokenService = tokenService;
        _accessRepository = accessRepository;
    }

    public async Task<IResult<Token>> HandleAsync(RefreshTokenCommand command)
    {
        var refreshTokenRequestValidationResult = 
            _tokenService.CheckForValidRefreshTokenRequest(command.AccessToken);

        if (refreshTokenRequestValidationResult.IsFailure)
        {
            return (IResult<Token>)refreshTokenRequestValidationResult;
        }

        var accessModel = 
            await _accessRepository.GetByIdAsync(command.RefreshToken);

        if (accessModel is null)
        {
            return Result.Error<Token>("AccessModel not found");
        }

        var allowedToRefreshResult = 
            accessModel.IsTokenAllowedToRefresh(command.AccessToken, command.AppId);

        if (allowedToRefreshResult.IsFailure)
        {
            return (IResult<Token>)allowedToRefreshResult;
        }

        var validRefreshAttemptResult = accessModel.CheckForValidRefreshAttempt();

        if (validRefreshAttemptResult.IsFailure)
        {
            await _accessRepository.RevokeAllTokensByUserIdAsync(accessModel.UserId);

            return (IResult<Token>)validRefreshAttemptResult;
        }

        accessModel.MakeTokenExpired();
        
        await _accessRepository.SaveAsync(accessModel);
        
        var userProfile = IdentityProvider.GetUserProfile(command.AccessToken);

        var refreshedAccessModel = 
            _tokenService.GenerateAccessModel(userProfile, command.AppId);
        
        await _accessRepository.SaveAsync(refreshedAccessModel);

        var token = refreshedAccessModel.ToToken();

        if (token is null)
        {
            return Result.Error<Token>("Token Creation Failed");
        }

        return Result.Success(token);
    }
} 