using Chat.Contact.Domain.Models;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Contact.Domain.Interfaces;

public interface IGroupMemberRepository : IRepository<GroupMemberModel>
{
    Task<bool> IsUserAlreadyExistsInGroupAsync(string groupId, string userId);

    Task<List<GroupMemberModel>> GetAllGroupMembers(string groupId);
}