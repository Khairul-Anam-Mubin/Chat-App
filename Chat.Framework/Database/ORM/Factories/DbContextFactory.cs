using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Database.ORM.MongoDb;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Framework.Database.ORM.Factories;

public class DbContextFactory : IDbContextFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DbContextFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IDbContext? GetDbContext(Context context)
    {
        return context switch
        {
            Context.Mongo => _serviceProvider.GetService<MongoDbContext>(),
            _ => _serviceProvider.GetService<MongoDbContext>()
        };
    }
}