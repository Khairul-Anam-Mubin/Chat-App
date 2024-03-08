using Chat.Framework.CQRS;
using Chat.Framework.Results;
using Chat.Identity.Application.Commands;
using Chat.Identity.Domain.Repositories;
using Chat.Identity.Domain.Services;

namespace Chat.Identity.Application.CommandHandlers;

public class LogOutCommandHandler : ICommandHandler<LogOutCommand>
{
    private readonly ITokenService _tokenService;
    private readonly IAccessRepository _accessRepository;

    public LogOutCommandHandler(ITokenService tokenService, IAccessRepository accessRepository)
    {
        _tokenService = tokenService;
        _accessRepository = accessRepository;
    }
    
    public async Task<IResult> HandleAsync(LogOutCommand command)
    {
        if (!await _accessRepository.RevokeAllTokenByAppIdAsync(command.AppId))
        {
            return Result.Error("Log out error");
        }

        return Result.Success("Logged out successfully!!");
    }
}