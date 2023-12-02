using Chat.FileStore.Domain.Interfaces;
using Chat.FileStore.Domain.Models;
using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Extensions;
using Microsoft.Extensions.Configuration;

namespace Chat.FileStore.Infrastructure.Repositories;

public class FileRepository : RepositoryBase<FileModel>, IFileRepository
{
    public FileRepository(IDbContext dbContext, IConfiguration configuration)
    : base(configuration.GetConfig<DatabaseInfo>()!, dbContext)
    {}
}