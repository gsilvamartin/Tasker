using Tasker.Logging.Interfaces;

namespace Tasker.Logging;

public enum LogLevel
{
    Information,
    Warning,
    Error,
    Critical
}

public sealed class TaskerLogger<T> : ITaskerLogger<T>
{
    public void LogInformation(string message)
    {
        Log(LogLevel.Information, message);
    }

    public void LogWarning(string message)
    {
        Log(LogLevel.Warning, message);
    }

    public void LogError(string message, Exception? exception = null)
    {
        Log(LogLevel.Error, message, exception);
    }

    public void LogCritical(string message, Exception? exception = null)
    {
        Log(LogLevel.Critical, message, exception);
    }

    private void Log(LogLevel level, string message, Exception? exception = null)
    {
        Console.WriteLine($"[Tasker] - [{level}] [{typeof(T).Name}]: {message}");

        if (exception is not null)
            Console.WriteLine($"Exception details: {exception}");
    }
}