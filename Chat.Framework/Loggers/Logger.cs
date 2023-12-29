namespace Chat.Framework.Loggers;

public class Logger : ILogger
{
    private readonly ILoggerChainProvider _chainProvider;

    public Logger(ILoggerChainProvider chainProvider)
    {
        _chainProvider = chainProvider;
    }

    public void Info(string message, params object[] data)
    {
        _chainProvider.GetLoggerChain().LogMessage(LogLevel.Info, message);
    }

    public void Error(string message, params object[] data)
    {
        _chainProvider.GetLoggerChain().LogMessage(LogLevel.Error, message);
    }

    public void Debug(string message, params object[] data)
    {
        _chainProvider.GetLoggerChain().LogMessage(LogLevel.Debug, message);
    }
}