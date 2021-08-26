using System;
using Spectre.Console;

namespace Syriaca.Client.Utils
{
    public class Logger
    {
        public static void Debug(string message)
        {
            if (DebugUtils.IsDebugBuild)
                log(message, LogLevel.Debug, "#808080");
        }

        public static void Info(string message)
        {
            log(message, LogLevel.Info, "#00FF7F");
        }

        public static void Log(string message)
        {
            log(message);
        }

        private static void log(string message, LogLevel level = LogLevel.Log, string color = "#00AAFF")
        {
            var currentTime = DateTime.Now.ToShortTimeString();
            var name = DebugUtils.GetCallingClass(1);

            AnsiConsole.MarkupLine(
                $"[{color}]•[/] [[{currentTime}]] [bold {color}][[{level}]][/] ｜ [[{name}]]: {message}");
        }

        private static void logBackground(string message, LogLevel level = LogLevel.Log,
                                          string color = "#00AAFF", string textColor = "white")
        {
            var currentTime = DateTime.Now.ToShortTimeString();
            var name = DebugUtils.GetCallingClass(1);

            AnsiConsole.MarkupLine(
                $"[{color}]•[/] [[{currentTime}]] [{textColor} bold on {color}][[{level}]][/] ｜ [[{name}]]: {message}");
        }

        public static void Warn(string message)
        {
            logBackground(message, LogLevel.Warning, "#FFC400", "black");
        }

        public static void Error(string message)
        {
            logBackground(message, LogLevel.Error, "#FF0040");
        }

        public static void Error(Exception exception)
        {
            Error(exception.Message);
        }

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