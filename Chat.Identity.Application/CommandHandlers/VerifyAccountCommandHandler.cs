using Chat.Framework.CQRS;
using Chat.Framework.Results;
using Chat.Identity.Application.Commands;
using Chat.Identity.Domain.Interfaces;

namespace Chat.Identity.Application.CommandHandlers;

public class VerifyAccountCommandHandler : ICommandHandler<VerifyAccountCommand>
{
    private readonly IUserRepository _userRepository;

    public VerifyAccountCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IResult> HandleAsync(VerifyAccountCommand request)
    {
        var userModel = await _userRepository.GetByIdAsync(request.UserId);

        if (userModel is null)
        {
            return Result.Error("Account verification error");
        }

        userModel.IsEmailVerified = true;

        await _userRepository.SaveAsync(userModel);

        return Result.Success("Account verified successfully.");
    }
}