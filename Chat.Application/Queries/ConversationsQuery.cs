using Chat.Application.DTOs;
using Peacious.Framework.CQRS;
using Peacious.Framework.Pagination;

namespace Chat.Application.Queries;

public class ConversationsQuery : APaginationQuery<ConversationDto>, IQuery<IPaginationResponse<ConversationDto>> { }