using Chat.Contact.Application.Interfaces;
using Chat.Contact.Domain.Commands;
using Chat.Contact.Domain.Models;
using Chat.Domain.Shared.Models;
using Chat.Domain.Shared.Queries;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Enums;
using Chat.Framework.Mediators;
using Chat.Framework.Proxy;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Contact.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<AddContactCommand, CommandResponse>), ServiceLifetime.Singleton)]
public class AddContactCommandHandler : ACommandHandler<AddContactCommand>
{
    private readonly IContactRepository _contactRepository;
    private readonly ICommandQueryProxy _commandQueryProxy;

    public AddContactCommandHandler(IContactRepository contactRepository, ICommandQueryProxy commandQueryProxy)
    {
        _contactRepository = contactRepository;
        _commandQueryProxy = commandQueryProxy;
    }

    protected override async Task<CommandResponse> OnHandleAsync(AddContactCommand command)
    {
        var response = command.CreateResponse();

        var userProfileQuery = new UserProfileQuery
        {
            UserIds = new List<string> { command.UserId },
            Emails = new List<string> { command.ContactEmail }
        };

        var queryResponse = await _commandQueryProxy.GetQueryResponseAsync(userProfileQuery);
        if (queryResponse == null || queryResponse.Status != ResponseStatus.Success || queryResponse.Items.Count < 2)
        {
            throw new Exception("User profile query error");
        }

        var userProfiles = queryResponse.GetItems<UserProfile>();

        var userProfile = userProfiles.FirstOrDefault(x => x.Id == command.UserId);
        if (userProfile == null)
        {
            throw new Exception("User profile error");
        }

        var contactUserProfile = userProfiles.FirstOrDefault(x => x.Email == command.ContactEmail);
        if (contactUserProfile == null)
        {
            throw new Exception("ContactModel User Profile error");
        }

        var userContact = new ContactModel
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userProfile.Id,
            ContactUserId = contactUserProfile.Id,
            CreatedAt = DateTime.UtcNow,
            IsPending = true
        };

        if (!await _contactRepository.SaveContactAsync(userContact))
        {
            throw new Exception("Saving User contact error");
        }

        response.Message = "ContactModel added successfully";

        return response;
    }
}