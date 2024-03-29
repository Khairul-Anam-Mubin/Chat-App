using Chat.Application.Shared.Providers;
using Chat.Framework.CQRS;
using Chat.Framework.Results;
using Chat.Identity.Application.Commands;
using Chat.Identity.Application.Dtos;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Domain.Repositories;
using Chat.Identity.Domain.Services;

namespace Chat.Identity.Application.CommandHandlers;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, TokenDto>
{
    private readonly ITokenService _tokenService;
    private readonly ITokenRepository _tokenRepository;

    public RefreshTokenCommandHandler(ITokenService tokenService, ITokenRepository tokenRepository)
    {
        _tokenService = tokenService;
        _tokenRepository = tokenRepository;
    }

    public async Task<IResult<TokenDto>> HandleAsync(RefreshTokenCommand command)
    {
        var refreshTokenRequestValidationResult = 
            _tokenService.CheckForValidRefreshTokenRequest(command.AccessToken);

        if (refreshTokenRequestValidationResult.IsFailure)
        {
            return Result.Error<TokenDto>(refreshTokenRequestValidationResult.Message);
        }

        var token = 
            await _tokenRepository.GetByIdAsync(command.RefreshToken);

        if (token is null)
        {
            return Result.Error<TokenDto>("TokenDto not found");
        }

        var allowedToRefreshResult = 
            token.IsTokenAllowedToRefresh(command.AccessToken, command.AppId);

        if (allowedToRefreshResult.IsFailure)
        {
            return Result.Error<TokenDto>(allowedToRefreshResult.Message);
        }

        var validRefreshAttemptResult = token.CheckForValidRefreshAttempt();

        if (validRefreshAttemptResult.IsFailure)
        {
            await _tokenRepository.RevokeAllTokensByUserIdAsync(token.UserId);

            return Result.Error<TokenDto>(validRefreshAttemptResult.Message);
        }

        token.MakeTokenExpired();
        
        await _tokenRepository.SaveAsync(token);
        
        var userProfile = IdentityProvider.GetUserProfile(command.AccessToken);

        var refreshedAccessModel = 
            _tokenService.GenerateToken(userProfile, command.AppId);
        
        await _tokenRepository.SaveAsync(refreshedAccessModel);

        var tokenDto = refreshedAccessModel.ToTokenDto();

        if (tokenDto is null)
        {
            return Result.Error<TokenDto>("Token Creation Failed");
        }

        return Result.Success(tokenDto);
    }
} 