using Chat.Contact.Application.Interfaces;
using Chat.Contact.Domain.Commands;
using Chat.Contact.Domain.Models;
using Chat.Domain.Shared.Queries;
using Chat.Framework.Attributes;
using Chat.Framework.Mediators;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Contact.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<AddContactCommand, Response>), ServiceLifetime.Transient)]
public class AddContactCommandHandler : IRequestHandler<AddContactCommand, Response>
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

    public async Task<Response> HandleAsync(AddContactCommand command)
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
            throw new Exception("User Not Exists");
        }

        var userProfiles = queryResponse.Profiles;

        var userProfile = userProfiles.First(x => x.Id == command.UserId);
        
        var contactUserProfile = userProfiles.First(x => x.Email == command.ContactEmail);

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
            throw new Exception("Contact Saving Failed");
        }

        response.Message = "Contact Added Successfully";

        return (Response)response;
    }
}