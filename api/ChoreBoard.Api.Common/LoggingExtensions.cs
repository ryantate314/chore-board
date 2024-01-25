using Microsoft.Extensions.Logging;
using System;

namespace ChoreBoard.Api.Common
{
    public static class LoggingExtensions
    {
        public static void LogDebugJson(this ILogger logger, object? obj)
        {
            LogJson(logger, LogLevel.Debug, obj, null);
        }

        public static void LogDebugJson(this ILogger logger, string message, object? obj)
        {
            LogJson(logger, LogLevel.Debug, obj, message);
        }

        private static void LogJson(ILogger logger, LogLevel logLevel, object? obj, string? message)
        {
            if (logger.IsEnabled(logLevel))
            {
                string json = System.Text.Json.JsonSerializer.Serialize(obj);
                logger.Log(logLevel, message != null ? message + ": " + json : json);
            }
        }
    }
}
