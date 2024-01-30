using Chat.Framework.CQRS;

namespace Chat.Contact.Application.Queries
{
    public class GroupQuery : IQuery
    {
        public string UserId { get; set; }

        public GroupQuery(string userId)
        {
            UserId = userId;
        }
    }
}
