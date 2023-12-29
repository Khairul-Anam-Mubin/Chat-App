namespace Chat.Framework.Loggers;

public class FileLogger : ALogger
{
    private readonly string _localLogFilePath;

    public FileLogger(LoggingConfig loggingConfig) : base(LogLevel.Debug)
    {
        _localLogFilePath = 
            Path.Join(loggingConfig.LocalLogFilePath, DateTime.UtcNow.Date.ToString("yyyy-MM-dd") + ".txt");
    }

    protected override void Log(LogLevel level, string message)
    {
        message = GetFormattedMessage(level, message);
        File.AppendAllTextAsync(_localLogFilePath, message + "\n").Wait();
    }
}