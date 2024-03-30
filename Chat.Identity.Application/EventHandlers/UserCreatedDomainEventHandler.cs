using Chat.Domain.Shared.Commands;
using Chat.Framework.CQRS;
using Chat.Framework.DDD;
using Chat.Framework.EmailSenders;
using Chat.Framework.Extensions;
using Chat.Framework.Identity;
using Chat.Identity.Application.Commands;
using Chat.Identity.Domain.DomainEvents;
using Microsoft.Extensions.Configuration;

namespace Chat.Identity.Application.EventHandlers;

public class UserCreatedDomainEventHandler : IDomainEventHandler<UserCreatedDomainEvent>
{
    private readonly ICommandService _commandService;
    private readonly IScopeIdentity _scopeIdentity;
    private readonly IConfiguration _configuration;

    public UserCreatedDomainEventHandler(IConfiguration configuration, ICommandService commandService, IScopeIdentity scopeIdentity)
    {
        _commandService = commandService;
        _scopeIdentity = scopeIdentity;
        _configuration = configuration;
    }

    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var userIdentity = _scopeIdentity.GetUser();

        if (userIdentity is null || userIdentity.Id != notification.Id)
        {
            Console.WriteLine("UserIdentifier Error in UserCreatedDomainEvent");
            return;
        }

        await SendVerificationEmailAsync(userIdentity);

        await TryGiveDeveloperAccessAsync(userIdentity);
    }

    private async Task SendVerificationEmailAsync(UserIdentity userIdentity)
    {
        if (string.IsNullOrEmpty(userIdentity.Email))
        {
            Console.WriteLine("UserIdentifier Email not found in UserCreatedDomainEvent. Send verification email failed");
            
            return;
        }

        var email = new Email
        {
            To = new List<string> { userIdentity.Email },
            IsHtmlContent = true,
            Content = $"<h1>Account Registration Successfully.</h1><br><button><a href=\"https://localhost:6001/api/UserProfile/verify-account?userId={userIdentity.Id}\">Click to verify account</a></button><br><br><p>Thanks</p>",
            Subject = "UserProfile registration complete"
        };

        await _commandService.SendAsync(new SendEmailCommand
        {
            Email = email
        });
    }

    private async Task TryGiveDeveloperAccessAsync(UserIdentity userIdentity)
    {
        var developerEmails = _configuration.GetConfig<List<string>>("DeveloperEmails");

        if (developerEmails is null) return;

        if (string.IsNullOrEmpty(userIdentity.Email)) return;

        if (!developerEmails.Contains(userIdentity.Email)) return;

        var developerAccessCommand = new GiveDeveloperAccessCommand
        {
            UserId = userIdentity.Id!
        };

        await _commandService.ExecuteAsync(developerAccessCommand);
    }
}
