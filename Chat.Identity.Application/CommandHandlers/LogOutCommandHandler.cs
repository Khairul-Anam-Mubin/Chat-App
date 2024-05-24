using Chat.Identity.Application.Commands;
using Chat.Identity.Domain.Repositories;
using Peacious.Framework.CQRS;
using Peacious.Framework.Results;

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