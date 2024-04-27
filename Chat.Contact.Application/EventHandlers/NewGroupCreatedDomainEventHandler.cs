using Chat.Contacts.Application.Commands;
using Chat.Contacts.Domain.DomainEvents;
using KCluster.Framework.CQRS;
using KCluster.Framework.DDD;

namespace Chat.Contacts.Application.EventHandlers;

public class NewGroupCreatedDomainEventHandler : IDomainEventHandler<NewGroupCreatedDomainEvent>
{
    private readonly ICommandExecutor _commandExecutor;

    public NewGroupCreatedDomainEventHandler(ICommandExecutor commandExecutor)
    {
        _commandExecutor = commandExecutor;
    }

    public async Task Handle(NewGroupCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var addMemberToGroupCommand = new AddMemberToGroupCommand(notification.Id, notification.CreatedBy, notification.CreatedBy);

        await _commandExecutor.ExecuteAsync(addMemberToGroupCommand);
    }
}
