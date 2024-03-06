using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Results;

namespace Chat.Contact.Domain.Models;

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

    public void AddMember(GroupMemberModel member)
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
            return Result.Error($"Member already exist in group : {Name}");
        }

        if (CreatedBy != addedBy)
        {
            return Result.Error($"Does not have permission to add member in group : {Name}");
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