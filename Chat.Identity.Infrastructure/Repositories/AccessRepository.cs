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

        public async Task<List<Permission>> GetPermissionsAsync(List<string> permissionIds)
        {
            var filterBuilder = new FilterBuilder<Permission>();

            var permissionIdsFilter = filterBuilder.In(x => x.Id, permissionIds);

            return await _dbContext.GetManyAsync<Permission>(_databaseInfo, permissionIdsFilter);
        }

        public async Task<List<Role>> GetRolesAsync(List<string> roleIds)
        {
            var filterBuilder = new FilterBuilder<Role>();

            var roleIdsFilter = filterBuilder.In(x => x.Id, roleIds);

            return await _dbContext.GetManyAsync<Role>(_databaseInfo, roleIdsFilter);
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
