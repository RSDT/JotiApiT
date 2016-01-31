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

        public static void Log(string id, string message, LogLevel logLevel)
        {
            bool log = false;
            switch(level)
            {
                case LogLevel.All:
                    log = true;
                    break;
                case LogLevel.Info:
                    log = (logLevel == level);
                    break;
                case LogLevel.Errors:
                    log = (logLevel == level);
                    break;
            }
            if (log) { Console.WriteLine("[" + id + "] [" + DateTime.Now + "] " + message); }
        }

        public enum LogLevel
        {
            Errors,
            Info,
            Debug,
            All
        }

    }
}
