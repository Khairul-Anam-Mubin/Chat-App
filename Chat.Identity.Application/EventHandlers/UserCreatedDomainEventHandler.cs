using Chat.Domain.Shared.Commands;
using Chat.Identity.Application.Commands;
using Chat.Identity.Domain.DomainEvents;
using Peacious.Framework.CQRS;
using Peacious.Framework.DDD;
using Peacious.Framework.EmailSenders;
using Peacious.Framework.Extensions;
using Peacious.Framework.Identity;
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

        if (IsDeveloper(userIdentity))
        {
            await GiveDeveloperAccessAsync(userIdentity);
        } 
        else
        {
            await GiveVisitorAccessAsync(userIdentity);
        }
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
            Content = $"<h1>Account Registration Successfully.</h1><br><button><a href=\"https://localhost:6001/api/User/verify-account?userId={userIdentity.Id}\">Click to verify account</a></button><br><br><p>Thanks</p>",
            Subject = "UserProfile registration complete"
        };

        await _commandService.SendAsync(new SendEmailCommand
        {
            Email = email
        });
    }

    private async Task GiveDeveloperAccessAsync(UserIdentity userIdentity)
    {
        var developerAccessCommand = new GiveDeveloperAccessCommand
        {
            UserId = userIdentity.Id!
        };

        await _commandService.ExecuteAsync(developerAccessCommand);
    }

    private async Task GiveVisitorAccessAsync(UserIdentity userIdentity)
    {
        var visitorAccessCommmand = new GiveVisitorAccessCommand
        {
            UserId = userIdentity.Id!
        };

        await _commandService.ExecuteAsync(visitorAccessCommmand);
    }

    private bool IsDeveloper(UserIdentity userIdentity)
    {
        var developerEmails = _configuration.GetConfig<List<string>>("DeveloperEmails");

        if (developerEmails is null) return false;

        if (string.IsNullOrEmpty(userIdentity.Email)) return false;

        return developerEmails.Contains(userIdentity.Email);
    }
}
