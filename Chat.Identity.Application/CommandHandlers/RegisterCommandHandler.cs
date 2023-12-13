using Chat.Domain.Shared.Commands;
using Chat.Framework.CQRS;
using Chat.Framework.EmailSenders;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Results;
using Chat.Identity.Application.Commands;
using Chat.Identity.Application.Extensions;
using Chat.Identity.Domain.Interfaces;
using Chat.Identity.Domain.Models;

namespace Chat.Identity.Application.CommandHandlers;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly ICommandBus _commandBus;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        ICommandBus commandBus)
    {
        _userRepository = userRepository;
        _commandBus = commandBus;
    }

    public async Task<IResult> HandleAsync(RegisterCommand command)
    {
        if (await _userRepository.IsUserExistAsync(command.UserModel))
        {
            return Result.Error("User email or id already exists!!");
        }
        
        command.UserModel.Id = Guid.NewGuid().ToString();
        command.UserModel.UserName = $"{command.UserModel.FirstName}_{command.UserModel.LastName}";
        command.UserModel.IsEmailVerified = false;

        if (!await _userRepository.SaveAsync(command.UserModel))
        {
            return Result.Error("Some anonymous problem occurred!!");
        }

        var response = Result.Success("User Created Successfully!!");

        await SendVerificationEmailAsync(command.UserModel);
        
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

        await _commandBus.SendAsync(new SendEmailCommand
        {
            Email = email
        });
    }
}