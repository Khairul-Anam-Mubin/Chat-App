using Chat.Contact.Domain.DomainEvents;
using Chat.Contact.Domain.Results;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.DDD;
using Chat.Framework.Results;

namespace Chat.Contact.Domain.Entities;

public class GroupModel : AggregateRoot, IRepositoryItem
{
    public string Name { get; private set; }
    public string CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private GroupModel(string name, string createdBy) : base(Guid.NewGuid().ToString())
    {
        Name = name;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    private List<GroupMemberModel> _members = new();

    private void AddMember(GroupMemberModel member)
    {
        _members ??= new List<GroupMemberModel>();
        _members.Add(member);
    }

    public List<GroupMemberModel> Members()
    {
        return _members.ToList();
    }

    public static IResult<GroupModel> Create(string name, string creatorId)
    {
        var group = new GroupModel(name, creatorId);

        group.RaiseDomainEvent(new NewGroupCreatedDomainEvent(group.Id, name, creatorId));

        return Result.Success(group);
    }

    public IResult AddNewMemberToGroup(string addedBy, string memberId, bool memberAlreadyExist)
    {
        if (memberAlreadyExist)
        {
            return Result.Error().MemberAlreadyExist();
        }

        if (CreatedBy != addedBy)
        {
            return Result.Error().GroupAddMemberPermissionProblem();
        }

        var groupMemberCreateResult = GroupMemberModel.Create(Id, memberId, CreatedBy);

        if (groupMemberCreateResult.IsFailure || groupMemberCreateResult.Value is null)
        {
            return groupMemberCreateResult;
        }

        AddMember(groupMemberCreateResult.Value);

        return Result.Success();
    }
}