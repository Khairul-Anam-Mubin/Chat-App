using Chat.FileStore.Domain.Interfaces;
using Chat.FileStore.Domain.Models;
using Chat.Framework.Attributes;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Database.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.FileStore.Persistence.Repositories;

[ServiceRegister(typeof(IFileRepository), ServiceLifetime.Singleton)]
public class FileRepository : RepositoryBase<FileModel>, IFileRepository
{
    public FileRepository(IMongoDbContext mongoDbContext, IConfiguration configuration)
    : base(configuration.GetSection("DatabaseInfo").Get<DatabaseInfo>(), mongoDbContext)
    {}
}