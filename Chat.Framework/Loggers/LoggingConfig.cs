﻿using Chat.Framework.ORM;

namespace Chat.Framework.Loggers;

public class LoggingConfig
{
    public DatabaseInfo DbConfig { get; set; }
    public string LocalLogFilePath { get; set; }
    public LogLevel LogLevel { get; set; }

    public LoggingConfig()
    {
        DbConfig = new DatabaseInfo();
        LocalLogFilePath = string.Empty;
    }
}