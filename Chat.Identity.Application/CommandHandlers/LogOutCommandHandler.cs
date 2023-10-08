using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Chat.Identity.Application.Interfaces;
using Chat.Identity.Domain.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Identity.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<LoginCommand, CommandResponse>), ServiceLifetime.Singleton)]
public class LogOutCommandHandler : ACommandHandler<LogOutCommand>
{
    private readonly ITokenService _tokenService;

    public LogOutCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
    
    protected override async Task<CommandResponse> OnHandleAsync(LogOutCommand command)
    {
        var response = command.CreateResponse();

        if (!await _tokenService.RevokeAllTokenByAppIdAsync(command.AppId))
        {
            throw new Exception("Log out error");
        }
        
        response.Message = "Logged out successfully!!";
        
        return response;
    }
}