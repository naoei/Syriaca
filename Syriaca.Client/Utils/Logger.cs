using System;
using Spectre.Console;

namespace Syriaca.Client.Utils
{
    /// <summary>
    /// A prettified logger specifically made for Syriaca, but can also be used for its plugins.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// The log level that should be bothered with.
        /// </summary>
        public static LogLevel Level = LogLevel.Debug;

        /// <summary>
        /// Prints out a debug message.
        /// </summary>
        /// <param name="message">The message to print out</param>
        public static void Debug(string message)
        {
            if (DebugUtils.IsDebugBuild)
                log(message, LogLevel.Debug, "#808080");
        }

        /// <summary>
        /// Prints out a info message
        /// </summary>
        /// <param name="message">The message to print out.</param>
        public static void Info(string message)
        {
            log(message, LogLevel.Info, "#00FF7F");
        }

        /// <summary>
        /// Prints out a log message
        /// </summary>
        /// <param name="message">The message to print out.</param>
        public static void Log(string message)
        {
            log(message);
        }

        private static void log(string message, LogLevel level = LogLevel.Log, string color = "#00AAFF")
        {
            if (!(Level <= level))
                return;

            var currentTime = DateTime.Now.ToShortTimeString();
            var name = DebugUtils.GetCallingClass(1);

            AnsiConsole.MarkupLine(
                $"[{color}]@[/] [[{currentTime}]] [bold {color}][[{level}]][/] ｜ [[{name}]]: {message}");
        }

        private static void logBackground(string message, LogLevel level = LogLevel.Log,
                                          string color = "#00AAFF", string textColor = "white")
        {
            if (!(Level <= level))
                return;

            var currentTime = DateTime.Now.ToShortTimeString();
            var name = DebugUtils.GetCallingClass(1);

            AnsiConsole.MarkupLine(
                $"[{color}]@[/] [[{currentTime}]] [{textColor} bold on {color}][[{level}]][/] ｜ [[{name}]]: {message}");
        }

        /// <summary>
        /// Prints out a warning message.
        /// </summary>
        /// <param name="message">The message to print out.</param>
        public static void Warn(string message)
        {
            logBackground(message, LogLevel.Warning, "#FFC400", "black");
        }

        /// <summary>
        /// Prints out an error message.
        /// </summary>
        /// <param name="message">The message to print out.</param>
        public static void Error(string message)
        {
            logBackground(message, LogLevel.Error, "#FF0040");
        }

        /// <summary>
        /// Prints out an error message
        /// </summary>
        /// <param name="exception">The exception to print out.</param>
        public static void Error(Exception exception)
        {
            Error(exception.Message);
        }

        /// <summary>
        /// Prints out an empty line.
        /// </summary>
        public static void Empty()
        {
            Console.WriteLine();
        }
    }

    public enum LogLevel
    {
        Debug,
        Info,
        Log,
        Warning,
        Error
    }
}