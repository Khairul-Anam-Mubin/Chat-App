using Chat.Domain.Shared.Commands;
using Chat.Framework.CQRS;
using Chat.Framework.DDD;
using Chat.Framework.EmailSenders;
using Chat.Identity.Domain.DomainEvents;
using Chat.Identity.Domain.Entities;

namespace Chat.Identity.Application.EventHandlers;

public class UserCreatedDomainEventHandler : IDomainEventHandler<UserCreatedDomainEvent>
{
    private readonly ICommandService _commandService;

    public UserCreatedDomainEventHandler(ICommandService commandService)
    {
        _commandService = commandService;
    }

    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var user = notification.User;

        await SendVerificationEmailAsync(user);
    }

    private async Task SendVerificationEmailAsync(User user)
    {
        var email = new Email
        {
            To = new List<string> { user.Email },
            IsHtmlContent = true,
            Content = $"<h1>Account Registration Successfully.</h1><br><button><a href=\"https://localhost:6001/api/User/verify-account?userId={user.Id}\">Click to verify account</a></button><br><br><p>Thanks</p>",
            Subject = "User registration complete"
        };

        await _commandService.SendAsync(new SendEmailCommand
        {
            Email = email
        });
    }
}
