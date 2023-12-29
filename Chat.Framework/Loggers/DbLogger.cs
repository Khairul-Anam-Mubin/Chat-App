using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Framework.Loggers;

public class DbLogger : ALogger
{
    private readonly IDbContext _dbContext;
    private readonly DatabaseInfo _databaseInfo;

    public DbLogger(LoggingConfig loggingConfig, IDbContextFactory dbContextFactory) 
        : base(LogLevel.Error)
    {
        _ = Enum.TryParse<Context>(loggingConfig.DbConfig.Provider, out var context);
        _databaseInfo = loggingConfig.DbConfig;
        _dbContext = dbContextFactory.GetDbContext(context);
    }

    protected override void Log(LogLevel level, string message)
    {
        var log = new Log(GetFormattedMessage(level, message));
        _dbContext.SaveAsync(_databaseInfo, log).Wait();
    }
}