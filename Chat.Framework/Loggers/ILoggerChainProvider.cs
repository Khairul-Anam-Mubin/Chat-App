namespace Chat.Framework.Loggers;

public interface ILoggerChainProvider
{
    ALogger GetLoggerChain();
}