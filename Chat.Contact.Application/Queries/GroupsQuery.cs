using Chat.Framework.CQRS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Contact.Application.Queries;

public class GroupsQuery : IQuery
{
    [Required]
    public List<string> GroupIds { get; set; }

    public GroupsQuery(List<string> groupIds)
    {
        GroupIds = groupIds;
    }
}
