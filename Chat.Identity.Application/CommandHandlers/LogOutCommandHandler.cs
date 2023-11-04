using Chat.Framework.Attributes;
using Chat.Framework.Mediators;
using Chat.Framework.Models;
using Chat.Identity.Application.Interfaces;
using Chat.Identity.Domain.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Identity.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<LogOutCommand, Response>), ServiceLifetime.Singleton)]
public class LogOutCommandHandler : IRequestHandler<LogOutCommand, Response>
{
    private readonly ITokenService _tokenService;

    public LogOutCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
    
    public async Task<Response> HandleAsync(LogOutCommand command)
    {
        var response = command.CreateResponse();

        if (!await _tokenService.RevokeAllTokenByAppIdAsync(command.AppId))
        {
            throw new Exception("Log out error");
        }
        
        response.Message = "Logged out successfully!!";
        
        return (Response)response;
    }
}