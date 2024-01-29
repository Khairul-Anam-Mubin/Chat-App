using Chat.Contact.Application.Commands;
using Chat.Contact.Domain.Interfaces;
using Chat.Contact.Domain.Models;
using Chat.Framework.CQRS;
using Chat.Framework.Results;

namespace Chat.Contact.Application.CommandHandlers;

public class CreateNewGroupCommandHandler : ICommandHandler<CreateNewGroupCommand>
{
    private readonly IGroupRepository _groupRepository;
    private readonly ICommandExecutor _commandExecutor;

    public CreateNewGroupCommandHandler(IGroupRepository groupRepository, ICommandExecutor commandExecutor)
    {
        _groupRepository = groupRepository;
        _commandExecutor = commandExecutor;
    }

    public async Task<IResult> HandleAsync(CreateNewGroupCommand request)
    {
        var groupName = request.GroupName;
        var userId = request.UserId;

        var groupModel = new GroupModel(groupName, userId);

        await _groupRepository.SaveAsync(groupModel);

        var addMemberToGroupCommand = new AddMemberToGroupCommand(groupModel.Id, userId, userId);

        await _commandExecutor.ExecuteAsync(addMemberToGroupCommand);

        var result = Result.Success("Group Created Successfully");
        
        result.SetData("GroupId", groupModel.Id);

        return result;
    }
}