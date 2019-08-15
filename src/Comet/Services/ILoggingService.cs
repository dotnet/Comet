using System;

namespace Comet.Services
{
    public enum LogType
    {
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }

    public interface ILoggingService
    {
        void Log(LogType logType, string message);
        void Log(LogType logType, string message, Exception exception);
    }
}
