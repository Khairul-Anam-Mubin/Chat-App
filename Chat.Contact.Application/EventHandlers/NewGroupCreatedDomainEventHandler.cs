using Chat.Contact.Application.Commands;
using Chat.Contact.Domain.DomainEvents;
using Chat.Framework.CQRS;
using Chat.Framework.DDD;

namespace Chat.Contact.Application.EventHandlers;

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
