using Chat.Contact.Domain.Results;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Results;

namespace Chat.Contact.Domain.Entities;

public class GroupModel : IEntity
{
    public string Id { get; set; }
    public string Name { get; private set; }
    public string CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private GroupModel(string name, string createdBy)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
        _members = new List<GroupMemberModel>();
    }

    private List<GroupMemberModel> _members;

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
        return Result.Success(new GroupModel(name, creatorId));
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