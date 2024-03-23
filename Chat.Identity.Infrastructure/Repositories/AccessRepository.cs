using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Database.ORM;
using Chat.Identity.Domain.Entities;
using Chat.Identity.Domain.Repositories;
using Chat.Framework.Database.ORM.Builders;

namespace Chat.Identity.Infrastructure.Repositories
{
    public class AccessRepository : IAccessRepository
    {
        private readonly IDbContext _dbContext;
        private readonly DatabaseInfo _databaseInfo;

        public AccessRepository(IDbContextFactory dbContextFactory, DatabaseInfo databaseInfo)
        {
            _dbContext = dbContextFactory.GetDbContext(Context.Mongo);
            _databaseInfo = databaseInfo;
        }

        public async Task<List<PermissionAccess>> GetUserPermissionsAsync(string userId)
        {
            var filterBuilder = new FilterBuilder<PermissionAccess>();

            var userIdFilter = filterBuilder.Eq(x => x.UserId, userId);

            return await _dbContext.GetManyAsync<PermissionAccess>(_databaseInfo, userIdFilter);
        }

        public async Task<List<RoleAccess>> GetUserRolesAsync(string userId)
        {
            var filterBuilder = new FilterBuilder<RoleAccess>();

            var userIdFilter = filterBuilder.Eq(x => x.UserId, userId);

            return await _dbContext.GetManyAsync<RoleAccess>(_databaseInfo, userIdFilter);
        }

        public async Task SaveUserPermissionsAsync(List<PermissionAccess> permissions)
        {
            await _dbContext.SaveManyAsync(_databaseInfo, permissions);
        }

        public async Task SaveUserRolesAsync(List<RoleAccess> roles)
        {
            await _dbContext.SaveManyAsync(_databaseInfo, roles);
        }
    }
}
