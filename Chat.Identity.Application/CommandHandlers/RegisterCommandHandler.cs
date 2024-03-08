using Chat.Domain.Shared.Commands;
using Chat.Framework.CQRS;
using Chat.Framework.EmailSenders;
using Chat.Framework.Results;
using Chat.Identity.Application.Commands;
using Chat.Identity.Domain.Entities;
using Chat.Identity.Domain.Repositories;

namespace Chat.Identity.Application.CommandHandlers;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly ICommandService _commandService;

    public RegisterCommandHandler(IUserRepository userRepository, ICommandService commandService)
    {
        _userRepository = userRepository;
        _commandService = commandService;
    }

    public async Task<IResult> HandleAsync(RegisterCommand command)
    {
        var userCreatedResult = 
            UserModel.Create(
                command.FirstName, 
                command.LastName, 
                command.BirthDay, 
                command.Email, 
                command.Password,
                await _userRepository.IsUserExistAsync(command.Email));

        if (userCreatedResult.IsFailure || userCreatedResult.Value is null)
        {
            return userCreatedResult;
        }

        var user = userCreatedResult.Value;

        if (!await _userRepository.SaveAsync(user))
        {
            return Result.Error("Some anonymous problem occurred!!");
        }

        var response = Result.Success("User Created Successfully!!");

        await SendVerificationEmailAsync(user);
        
        return response;
    }

    private async Task SendVerificationEmailAsync(UserModel userModel)
    {
        var email = new Email
        {
            To = new List<string> { userModel.Email },
            IsHtmlContent = true,
            Content = $"<h1>Account Registration Successfully.</h1><br><button><a href=\"https://localhost:6001/api/User/verify-account?userId={userModel.Id}\">Click to verify account</a></button><br><br><p>Thanks</p>",
            Subject = "User registration complete"
        };

        await _commandService.SendAsync(new SendEmailCommand
        {
            Email = email
        });
    }
}