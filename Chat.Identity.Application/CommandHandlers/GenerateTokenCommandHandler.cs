using Chat.Identity.Application.Commands;
using Chat.Identity.Application.Dtos;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Domain.Repositories;
using Chat.Identity.Domain.Services;
using Peacious.Framework.CQRS;
using Peacious.Framework.Results;

namespace Chat.Identity.Application.CommandHandlers;

public class GenerateTokenCommandHandler : ICommandHandler<GenerateTokenCommand, TokenDto>
{
    private readonly IUserAccessRepository _userAccessRepository;
    private readonly ITokenService _tokenService;
    private readonly ITokenRepository _tokenRepository;
    private readonly IUserRepository _userRepository;

    public GenerateTokenCommandHandler(IUserAccessRepository userAccessRepository, ITokenService tokenService, ITokenRepository tokenRepository, IUserRepository userRepository)
    {
        _userAccessRepository = userAccessRepository;
        _tokenService = tokenService;
        _tokenRepository = tokenRepository;
        _userRepository = userRepository;
    }

    public async Task<IResult<TokenDto>> HandleAsync(GenerateTokenCommand command)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId);

        if (user is null)
        {
            return Result.Error<TokenDto>("User not found");
        }

        var userAccess = 
            await _userAccessRepository.GetUserAccessByUserIdAsync(command.UserId);

        if (userAccess is null)
        {
            return Result.Error<TokenDto>("User Access not found.");
        }

        var token = _tokenService.GenerateToken(user, userAccess, command.AppId);

        await _tokenRepository.SaveAsync(token);

        var tokenDto = token.ToTokenDto();

        if (tokenDto is null)
        {
            return Result.Error<TokenDto>("Token Creation Failed");
        }

        return Result.Success(tokenDto);
    }
}
