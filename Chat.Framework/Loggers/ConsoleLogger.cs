namespace Chat.Framework.Loggers;

public class ConsoleLogger : ALogger
{
    public ConsoleLogger() : base(LogLevel.Info) {}

    protected override void Log(LogLevel level, string message)
    {
        Console.WriteLine($"{level} :: {message}");
    }
}