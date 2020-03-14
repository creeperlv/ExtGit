using System;
using System.Collections.Generic;
using System.Text;

namespace ExtGit.Core.Utilities
{
    public class Debugger : IDebugger
    {
        public static LogLevel MinLogLevel = LogLevel.Normal;
        public static IDebugger CurrentDebugger = new Debugger();
        public void Log(string msg)
        {
            Log(msg, LogLevel.Normal);
        }

        public void Log(string msg, LogLevel logLevel)
        {
            if(logLevel>MinLogLevel)
            switch (logLevel)
            {
                case LogLevel.Development:
                    {
                        if (Features.DevelopmentLog)
                        {
                            Console.WriteLine(msg);
                        }
                    }
                    break;
                case LogLevel.Normal:
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    break;
                case LogLevel.Warning:
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    break;
                case LogLevel.Error:
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    break;
                default:
                    break;
            }
        }
    }
    public interface IDebugger
    {
        void Log(string msg);
        void Log(string msg,LogLevel logLevel);
    }
    public enum LogLevel
    {
        Development,Normal,Warning,Error
    }
}
