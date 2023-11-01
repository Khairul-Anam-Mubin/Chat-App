using Chat.Contact.Application.Interfaces;
using Chat.Contact.Domain.Commands;
using Chat.Contact.Domain.Models;
using Chat.Domain.Shared.Queries;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Mediators;
using Chat.Framework.MessageBrokers;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Contact.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<AddContactCommand, CommandResponse>), ServiceLifetime.Transient)]
public class AddContactCommandHandler : IRequestHandler<AddContactCommand, CommandResponse>
{
    private readonly IContactRepository _contactRepository;
    private readonly IMessageRequestClient _messageRequestClient;

    public AddContactCommandHandler(
        IContactRepository contactRepository, 
        IMessageRequestClient messageRequestClient)
    {
        _contactRepository = contactRepository;
        _messageRequestClient = messageRequestClient;
    }

    public async Task<CommandResponse> HandleAsync(AddContactCommand command)
    {
        var response = command.CreateResponse();

        var userProfileQuery = new UserProfileQuery
        {
            UserIds = new List<string> { command.UserId },
            Emails = new List<string> { command.ContactEmail }
        };

        var queryResponse = 
            await _messageRequestClient.GetResponseAsync<UserProfileQuery, UserProfileQueryResponse>(userProfileQuery);
        
        if (queryResponse == null || queryResponse.Profiles.Count < 2)
        {
            throw new Exception("User profile query error");
        }

        var userProfiles = queryResponse.Profiles;

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

        if (!await _contactRepository.SaveAsync(userContact))
        {
            throw new Exception("Saving User contact error");
        }

        response.Message = "ContactModel added successfully";

        return response;
    }
}