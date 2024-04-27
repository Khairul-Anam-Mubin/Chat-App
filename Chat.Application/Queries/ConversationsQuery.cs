using Chat.Application.DTOs;
using KCluster.Framework.CQRS;
using KCluster.Framework.Pagination;

namespace Chat.Application.Queries;

public class ConversationsQuery : APaginationQuery<ConversationDto>, IQuery {}