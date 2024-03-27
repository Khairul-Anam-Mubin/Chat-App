using Chat.Framework.ORM.Enums;
using Chat.Framework.ORM.Interfaces;
using Chat.Framework.ORM.MongoDb;
using Chat.Framework.ORM.Sql;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Framework.ORM.Factories;

public class DbContextFactory : IDbContextFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DbContextFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IDbContext GetDbContext(Context context)
    {
        return context switch
        {
            Context.Mongo => _serviceProvider.GetRequiredService<MongoDbContext>(),
            Context.Sql => _serviceProvider.GetRequiredService<SqlDbContext>(),
            _ => _serviceProvider.GetRequiredService<MongoDbContext>()
        };
    }
}