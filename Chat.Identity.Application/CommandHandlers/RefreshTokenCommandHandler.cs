using Chat.Application.Shared.Providers;
using Chat.Identity.Application.Commands;
using Chat.Identity.Application.Dtos;
using Chat.Identity.Domain.Repositories;
using Chat.Identity.Domain.Services;
using Peacious.Framework.CQRS;
using Peacious.Framework.Results;

namespace Chat.Identity.Application.CommandHandlers;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, TokenDto>
{
    private readonly ITokenService _tokenService;
    private readonly ITokenRepository _tokenRepository;
    private readonly ICommandService _commandService;

    public RefreshTokenCommandHandler(ITokenService tokenService, ITokenRepository tokenRepository, ICommandService commandService)
    {
        _tokenService = tokenService;
        _tokenRepository = tokenRepository;
        _commandService = commandService;
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

        var generateTokenCommand = new GenerateTokenCommand
        {
            UserId = userProfile.Id,
            AppId = command.AppId
        };

        var generateTokenResult =
            await _commandService.ExecuteAsync<GenerateTokenCommand, TokenDto>(generateTokenCommand);

        var tokenDto = generateTokenResult.Value;

        if (generateTokenResult.IsFailure || tokenDto is null)
        {
            return generateTokenResult;
        }

        return Result.Success(tokenDto);
    }
} 