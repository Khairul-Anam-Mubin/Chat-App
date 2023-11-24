using Chat.Framework.Mediators;
using Chat.Framework.RequestResponse;
using Chat.Identity.Application.Commands;
using Chat.Identity.Domain.Interfaces;

namespace Chat.Identity.Application.CommandHandlers;

public class LogOutCommandHandler : IHandler<LogOutCommand, IResponse>
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