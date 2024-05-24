using Peacious.Framework.DDD;
using Peacious.Framework.EDD;
using Peacious.Framework.Extensions;
using Peacious.Framework.ORM;
using Peacious.Framework.ORM.Builders;
using Peacious.Framework.ORM.Enums;
using Peacious.Framework.ORM.Interfaces;
using Chat.Identity.Domain.Entities;
using Chat.Identity.Domain.Repositories;
using Microsoft.Extensions.Configuration;

namespace Chat.Identity.Infrastructure.Repositories;

public class RoleRepository : RepositoryBaseWrapper<Role>, IRoleRepository
{
    public RoleRepository(IConfiguration configuration, IDbContextFactory dbContextFactory, IEventService eventService)
        : base(configuration.TryGetConfig<DatabaseInfo>("DatabaseInfo"), dbContextFactory.GetDbContext(Context.Mongo), eventService) {}

    public async Task<Role?> GetRoleByTitleAsync(string title)
    {
        var titleFilter = new FilterBuilder<Role>().Eq(role => role.Title, title);
        
        return await DbContext.GetOneAsync<Role>(DatabaseInfo, titleFilter);
    }

    public async Task<List<Role>> GetRolesByTitlesAsync(List<string> titles)
    {
        var titleFilter = new FilterBuilder<Role>().In(role => role.Title, titles);

        return await DbContext.GetManyAsync<Role>(DatabaseInfo, titleFilter);
    }
}
