using KCluster.Framework.DDD;
using KCluster.Framework.EDD;
using KCluster.Framework.Extensions;
using KCluster.Framework.ORM;
using KCluster.Framework.ORM.Builders;
using KCluster.Framework.ORM.Enums;
using KCluster.Framework.ORM.Interfaces;
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
