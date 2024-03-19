using Chat.Framework.CQRS;
using Chat.Framework.Results;
using Chat.Identity.Application.Commands;
using Chat.Identity.Domain.Repositories;

namespace Chat.Identity.Application.CommandHandlers;

public class LogOutCommandHandler : ICommandHandler<LogOutCommand>
{
    private readonly ITokenRepository _tokenRepository;

    public LogOutCommandHandler(ITokenRepository tokenRepository)
    {
        _tokenRepository = tokenRepository;
    }
    
    public async Task<IResult> HandleAsync(LogOutCommand command)
    {
        if (!await _tokenRepository.RevokeAllTokenByAppIdAsync(command.AppId))
        {
            return Result.Error("Log out error");
        }

        return Result.Success("Logged out successfully!!");
    }
}