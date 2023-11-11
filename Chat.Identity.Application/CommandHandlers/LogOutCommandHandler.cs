using Chat.Framework.Attributes;
using Chat.Framework.Interfaces;
using Chat.Framework.Mediators;
using Chat.Framework.Models;
using Chat.Identity.Domain.Commands;
using Chat.Identity.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Identity.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<LogOutCommand, IResponse>), ServiceLifetime.Singleton)]
public class LogOutCommandHandler : IRequestHandler<LogOutCommand, IResponse>
{
    private readonly ITokenService _tokenService;

    public LogOutCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
    
    public async Task<IResponse> HandleAsync(LogOutCommand command)
    {
        var response = Response.Success();

        if (!await _tokenService.RevokeAllTokenByAppIdAsync(command.AppId))
        {
            throw new Exception("Log out error");
        }
        
        response.Message = "Logged out successfully!!";
        
        return response;
    }
}