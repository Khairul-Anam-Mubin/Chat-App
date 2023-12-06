using Chat.Framework.Mediators;
using Chat.Framework.RequestResponse;
using Chat.Identity.Application.Commands;
using Chat.Identity.Domain.Interfaces;

namespace Chat.Identity.Application.CommandHandlers;

public class VerifyAccountCommandHandler : IHandler<VerifyAccountCommand, IResponse>
{
    private readonly IUserRepository _userRepository;

    public VerifyAccountCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IResponse> HandleAsync(VerifyAccountCommand request)
    {
        var userModel = await _userRepository.GetByIdAsync(request.UserId);

        if (userModel == null)
        {
            return Response.Error("Account verification error");
        }

        userModel.IsEmailVerified = true;

        await _userRepository.SaveAsync(userModel);

        return Response.Success("Account verified successfully.");
    }
}