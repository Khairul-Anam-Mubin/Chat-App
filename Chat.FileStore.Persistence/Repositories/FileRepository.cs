using Chat.FileStore.Domain.Interfaces;
using Chat.FileStore.Domain.Models;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Database.Repositories;
using Chat.Framework.Extensions;
using Microsoft.Extensions.Configuration;

namespace Chat.FileStore.Infrastructure.Repositories;

public class FileRepository : RepositoryBase<FileModel>, IFileRepository
{
    public FileRepository(IMongoDbContext mongoDbContext, IConfiguration configuration)
    : base(configuration.GetConfig<DatabaseInfo>()!, mongoDbContext)
    {}
}