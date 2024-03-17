using Chat.FileStore.Domain.Entities;
using Chat.FileStore.Domain.Repositories;
using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.FileStore.Infrastructure.Repositories;

public class FileDirectoryRepository : RepositoryBase<FileDirectory>, IFileDirectoryRepository
{
    public FileDirectoryRepository(IDbContextFactory dbContextFactory, DatabaseInfo databaseInfo)
        : base(databaseInfo, dbContextFactory.GetDbContext(Context.Mongo))
    { }
}