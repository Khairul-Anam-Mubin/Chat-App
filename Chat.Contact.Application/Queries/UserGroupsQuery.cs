using Chat.Framework.CQRS;
using System.ComponentModel.DataAnnotations;

namespace Chat.Contacts.Application.Queries
{
    public class UserGroupsQuery : IQuery
    {
        [Required]
        public string UserId { get; set; }

        public UserGroupsQuery(string userId)
        {
            UserId = userId;
        }
    }
}
