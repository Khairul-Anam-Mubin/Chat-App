using Chat.Contacts.Domain.DomainEvents;
using Chat.Contacts.Domain.Results;
using Chat.Framework.DDD;
using Chat.Framework.ORM.Interfaces;
using Chat.Framework.Results;

namespace Chat.Contacts.Domain.Entities;

public class Group : AggregateRoot, IRepositoryItem
{
    public string Name { get; private set; }
    public string CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Group(string name, string createdBy) : base(Guid.NewGuid().ToString())
    {
        Name = name;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    private List<GroupMember> _members = new();

    private void AddMember(GroupMember member)
    {
        _members ??= new();
        _members.Add(member);
    }

    public List<GroupMember> Members
    {
        get
        {
            _members ??= new();
            return _members.ToList();
        }
    }

    public static IResult<Group> Create(string name, string creatorId)
    {
        var group = new Group(name, creatorId);

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

        var groupMemberCreateResult = GroupMember.Create(Id, memberId, CreatedBy);

        if (groupMemberCreateResult.IsFailure || groupMemberCreateResult.Value is null)
        {
            return groupMemberCreateResult;
        }

        AddMember(groupMemberCreateResult.Value);

        return Result.Success();
    }
}