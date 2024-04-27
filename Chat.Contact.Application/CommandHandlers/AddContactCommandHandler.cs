using Chat.Contacts.Application.Commands;
using Chat.Contacts.Domain.Entities;
using Chat.Contacts.Domain.Repositories;
using Chat.Contacts.Domain.Results;
using Chat.Domain.Shared.Queries;
using KCluster.Framework.CQRS;
using KCluster.Framework.Identity;
using KCluster.Framework.Results;

namespace Chat.Contacts.Application.CommandHandlers;

public class AddContactCommandHandler : ICommandHandler<AddContactCommand>
{
    private readonly IContactRepository _contactRepository;
    private readonly IScopeIdentity _scopeIdentity;
    private readonly IQueryService _queryService;

    public AddContactCommandHandler(
        IContactRepository contactRepository,
        IScopeIdentity scopeIdentity,
        IQueryService queryService)
    {
        _contactRepository = contactRepository;
        _scopeIdentity = scopeIdentity;
        _queryService = queryService;
    }

    public async Task<IResult> HandleAsync(AddContactCommand command)
    {
        var userId = _scopeIdentity.GetUserId()!;

        var userProfileQuery = new UserProfileQuery
        {
            UserIds = new List<string> { userId },
            Emails = new List<string> { command.ContactEmail }
        };

        var queryResponse =
            await _queryService.GetResponseAsync<UserProfileQuery, UserProfileResponse>(userProfileQuery);

        if (queryResponse.Profiles.Count < 2)
        {
            return Result.Error().GetUserFailed();
        }

        var userProfiles = queryResponse.Profiles;

        var userProfile = userProfiles.First(x => x.Id == userId);

        var contactUserProfile = userProfiles.First(x => x.Email == command.ContactEmail);

        var userContact = Contact.Create(userProfile.Id, contactUserProfile.Id);

        if (!await _contactRepository.SaveAsync(userContact))
        {
            return Result.Error().ContactSaveProblem();
        }

        return Result.Success().ContactAdded();
    }
}