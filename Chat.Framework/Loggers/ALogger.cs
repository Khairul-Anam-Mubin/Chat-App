namespace Chat.Framework.Loggers;

public abstract class ALogger
{
    protected LogLevel LogLevel;
    private ALogger? _nextLogger;

    protected ALogger(LogLevel level)
    {
        LogLevel = level;
    }

    public void SetNext(ALogger logger)
    {
        _nextLogger = logger;
    }

    public ALogger? GetNext()
    {
        return _nextLogger;
    }

    protected string GetFormattedMessage(LogLevel level, string message)
    {
        return $"{level} :: Time({DateTime.UtcNow}) -- {message}";
    }

    public void LogMessage(LogLevel level, string message)
    {
        if (LogLevel <= level)
        {
            Log(level, message);
        }

        GetNext()?.LogMessage(level, message);
    }

    protected abstract void Log(LogLevel level, string message);
}