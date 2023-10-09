using Chat.FileStore.Application.Interfaces;
using Chat.FileStore.Domain.Models;
using Chat.Framework.Attributes;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.FileStore.Persistence.Repositories;

[ServiceRegister(typeof(IFileRepository), ServiceLifetime.Singleton)]
public class FileRepository : IFileRepository
{
    private readonly DatabaseInfo _databaseInfo;
    private readonly IMongoDbContext _dbContext;

    public FileRepository(IMongoDbContext mongoDbContext, IConfiguration configuration)
    {
        _databaseInfo = configuration.GetSection("DatabaseInfo").Get<DatabaseInfo>();
        _dbContext = mongoDbContext;
    }

    public async Task<bool> SaveFileModelAsync(FileModel fileModel)
    {
        return await _dbContext.SaveAsync(_databaseInfo, fileModel);
    }

    public async Task<FileModel?> GetFileModelByIdAsync(string id)
    {
        return await _dbContext.GetByIdAsync<FileModel>(_databaseInfo, id);
    }
}