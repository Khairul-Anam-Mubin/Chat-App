using Chat.Framework.CQRS;
using Chat.Framework.Results;
using Chat.Identity.Application.Commands;
using Chat.Identity.Domain.Interfaces;

namespace Chat.Identity.Application.CommandHandlers;

public class LogOutCommandHandler : ICommandHandler<LogOutCommand>
{
    private readonly ITokenService _tokenService;

    public LogOutCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
    
    public async Task<IResult> HandleAsync(LogOutCommand command)
    {
        if (!await _tokenService.RevokeAllTokenByAppIdAsync(command.AppId))
        {
            return Result.Error("Log out error");
        }

        return Result.Success("Logged out successfully!!");
    }
}