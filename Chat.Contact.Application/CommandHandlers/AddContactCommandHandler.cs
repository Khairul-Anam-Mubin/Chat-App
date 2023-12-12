using Chat.Contact.Application.Commands;
using Chat.Contact.Domain.Interfaces;
using Chat.Contact.Domain.Models;
using Chat.Domain.Shared.Queries;
using Chat.Framework.CQRS;
using Chat.Framework.MessageBrokers;
using Chat.Framework.Results;

namespace Chat.Contact.Application.CommandHandlers;

public class AddContactCommandHandler : 
    ICommandHandler<AddContactCommand>
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

    public async Task<IResult> HandleAsync(AddContactCommand command)
    {
        var response = Result.Success();

        var userProfileQuery = new UserProfileQuery
        {
            UserIds = new List<string> { command.UserId },
            Emails = new List<string> { command.ContactEmail }
        };

        var queryResponse = 
            await _messageRequestClient.GetResponseAsync<UserProfileQuery, UserProfileResponse>(userProfileQuery);
        
        if (queryResponse.Profiles.Count < 2)
        {
            return Result.Error("User Not Exists");
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
            return Result.Error("Contact Saving Failed");
        }

        response.Message = "Contact Added Successfully";

        return response;
    }
}