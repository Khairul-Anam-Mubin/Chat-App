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
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Contact.Application.CommandHandlers;

[ServiceRegister(typeof(IRequestHandler<AddContactCommand, CommandResponse>), ServiceLifetime.Transient)]
public class AddContactCommandHandler : ACommandHandler<AddContactCommand>
{
    private readonly IContactRepository _contactRepository;
    private readonly ICommandQueryProxy _commandQueryProxy;
    private readonly IRequestClient<UserProfileQuery> _requestClient;

    public AddContactCommandHandler(
        IContactRepository contactRepository, 
        ICommandQueryProxy commandQueryProxy, 
        IRequestClient<UserProfileQuery> requestClient)
    {
        _contactRepository = contactRepository;
        _commandQueryProxy = commandQueryProxy;
        _requestClient = requestClient;
    }

    protected override async Task<CommandResponse> OnHandleAsync(AddContactCommand command)
    {
        var response = command.CreateResponse();
        var userProfileQuery = new UserProfileQuery
        {
            UserIds = new List<string> { command.UserId },
            Emails = new List<string> { command.ContactEmail }
        };

        var res = await _requestClient.GetResponse<UserProfileQueryResponse>(userProfileQuery);
        
        // var queryResponse = await _commandQueryProxy.GetQueryResponseAsync(userProfileQuery);
        var queryResponse = res.Message;
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