namespace Chat.Framework.Loggers;

public interface ILogger
{
    void Info(string message, params object[] data);
    void Error(string message, params object[] data);
    void Debug(string message, params object[] data);
}