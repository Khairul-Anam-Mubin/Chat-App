namespace Chat.Framework.Loggers;

public class LoggerChainProvider : ILoggerChainProvider
{
    private readonly ALogger _consoleLogger;
    private readonly ALogger _fileLogger;
    private readonly ALogger _dbLogger;

    private readonly LoggingConfig _loggingConfig;

    public LoggerChainProvider(
        ConsoleLogger consoleLogger,
        FileLogger fileLogger,
        DbLogger dbLogger,
        LoggingConfig loggingConfig)
    {
        _loggingConfig = loggingConfig;
        _consoleLogger = consoleLogger;
        _fileLogger = fileLogger;
        _dbLogger = dbLogger;

        _dbLogger.SetNext(_fileLogger);
        _fileLogger.SetNext(_consoleLogger);
    }

    public ALogger GetLoggerChain()
    {
        return _loggingConfig.LogLevel switch
        {
            LogLevel.Info => _consoleLogger,
            LogLevel.Debug => _fileLogger,
            LogLevel.Error => _dbLogger,
            _ => _consoleLogger
        };
    }
}