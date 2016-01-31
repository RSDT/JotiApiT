using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JotiApiT
{
    public static class Logger
    {

        public static LogLevel level = LogLevel.All;

        public static ConsoleColor InfoColor = ConsoleColor.DarkYellow;

        public static ConsoleColor DebugColor = ConsoleColor.DarkCyan;

        public static ConsoleColor ErrorColor = ConsoleColor.Red;

        public static void Log(string id, string message, LogLevel logLevel)
        {
            bool shouldLog = false;

            switch(logLevel)
            {
                case LogLevel.Info:
                    Console.ForegroundColor = InfoColor;
                    break;
                case LogLevel.Debug:
                    Console.ForegroundColor = DebugColor;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ErrorColor;
                    break;
            }

            shouldLog = (level == LogLevel.All || logLevel == level);

            if (shouldLog) {
                Console.WriteLine("[" + id + "] [" + logLevel + "] [" + DateTime.Now + "] " + message);
                Console.ResetColor();
            }
        }

        public enum LogLevel
        {
            Error,
            Info,
            Debug,
            All
        }

    }
}
