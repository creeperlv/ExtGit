using ExtGit.Core;
using ExtGit.Core.Utilities;
using ExtGit.Core.Version1;
using ExtGit.Localization;
using System;
using System.IO;

namespace ExtGit
{
    class Program
    {
        static Operation CurrentOperation = Operation.None;
        static Version ShellVersion = new Version(1, 0, 1, 0);
        static void Main(string[] args)
        {
            /**
             * 
             * Layout:
             *      Repo/
             *      Repo/<WorkSpace>
             *      Repo/.ExtGit/
             *      Repo/.ExtGit/<ExtGitWorkLoad>
             *      Repo/.ExtGit/.extgitconfigs/ - ExtGit Configurations
             *      Repo/.ExtGit/.git/...
             * 
             **/
            GraftOptions options = new GraftOptions();
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToUpper())
                {
                    case "--GRAFT-LOSE-GIT-HISTORY":
                        options.KeepGitHistory = false;
                        break;
                    case "--GRAFT-HOLD-COMMIT":
                        options.CommitAfterGraft = false;
                        break;
                    case "-C":
                    case "COMMIT":
                        CurrentOperation = Operation.Commit;
                        break;
                    case "-O":
                    case "CHECKOUT":
                        CurrentOperation = Operation.Checkout;
                        break;
                    case "-H":
                    case "-?":
                    case "--?":
                    case "-HELP":
                    case "--HELP":
                    case "HELP":
                        CurrentOperation = Operation.Help;
                        break;
                    case "-V":
                    case "--V":
                    case "VERSION":
                    case "-VERSION":
                    case "--VERSION":
                    case "VER":
                    case "-VER":
                    case "--VER":
                        CurrentOperation = Operation.Version;
                        break;
                    case "NEW":
                    case "INIT":
                    case "-INIT":
                    case "--I":
                    case "--N":
                        CurrentOperation = Operation.Create;
                        break;
                    case "TEST":
                    case "-TEST":
                    case "--T":
                    case "-T":
                        CurrentOperation = Operation.FunctionTest;
                        break;
                    case "GRAFT":
                    case "-G":
                    case "--G":
                        CurrentOperation = Operation.Graft;
                        break;
                    case "--LOG-LEVEL":
                    case "-LOG-LEVEL":
                    case "-L":
                    case "--L":
                        var logLevel = args[i + 1];
                        i++;
                        switch (logLevel.ToUpper())
                        {
                            case "FULL":
                                Debugger.MinLogLevel = LogLevel.Development;
                                break;
                            case "STANDARD":
                            case "STD":
                                Debugger.MinLogLevel = LogLevel.Normal;
                                break;
                            case "WARNING":
                                Debugger.MinLogLevel = LogLevel.Warning;
                                break;
                            case "ERROR":
                                Debugger.MinLogLevel = LogLevel.Error;
                                break;

                            default:
                                break;
                        }
                        break;
                    default:
                        Console.WriteLine("Unknown parameter:" + args[i]);
                        break;
                }
            }
            Console.WriteLine(Language.CurrentLanguage.Get("TITLE0", "ExtGit - Designed for commit large file without GIT-LFS to host platforms."));
            Console.WriteLine(Language.CurrentLanguage.Get("PREVIEW00", "This software still in an early preview"));
            Console.WriteLine("");
            Console.WriteLine(Language.CurrentLanguage.Get("TITLE1", "Designed for What Happened to Site-13?"));
            switch (CurrentOperation)
            {
                case Operation.Commit:
                    {
                        try
                        {

                            double prog = 0.0;
                        Repo r = new Repo(new DirectoryInfo("./").FullName);
                        r.Commit(ref prog);
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(Language.CurrentLanguage.Get("FATAL", "Fatal:"));
                            Console.ForegroundColor = ConsoleColor.White;
                            Debugger.CurrentDebugger.Log(e.Message, LogLevel.Error);
                        }
                    }
                    break;
                case Operation.Checkout:
                    {

                        try
                        {

                            double prog = 0.0;
                            Repo r = new Repo(new DirectoryInfo("./").FullName);
                            r.Checkout(ref prog);
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(Language.CurrentLanguage.Get("FATAL", "Fatal:"));
                            Console.ForegroundColor = ConsoleColor.White;
                            Debugger.CurrentDebugger.Log(e.Message, LogLevel.Error);
                        }
                    }
                    break;
                case Operation.None:
                case Operation.Help:
                    ShowHelp();
                    break;
                case Operation.FunctionTest:
                    {
                        Tester.Test00();
                    }
                    break;
                case Operation.Graft:
                    {
                        Repo.Graft(new DirectoryInfo(".").FullName,options,ShellVersion);
                    }
                    break;
                case Operation.Version:
                    ShowVersion();
                    break;
                case Operation.Create:
                    {
                        Repo repo = new Repo();
                        repo.ExtGitVer = ShellVersion;
                        repo.ExtGitVerCore = CoreLib.CurrentCore.CoreVersion;
                        Repo.Create(repo, new DirectoryInfo("./").FullName);
                    }
                    break;
                default:
                    break;
            }
        }

        private static void ShowVersion()
        {
            Console.WriteLine("");
            Console.WriteLine(Language.CurrentLanguage.Get("VERINFOSTR00", "ExtGit Components:"));
            Console.WriteLine("");
            Console.WriteLine($"\t{Language.CurrentLanguage.Get("VERINFOSTR03", "Shell Version:")}\t" + ShellVersion.ToString() + $"({ShellVersion.Build})");
            Console.WriteLine($"\t{Language.CurrentLanguage.Get("VERINFOSTR04", "Core Version:")}\t" + CoreLib.CurrentCore.CoreVersion.ToString() + $"({CoreLib.CurrentCore.CoreVersion.Build})");
            Console.WriteLine("");
            Console.WriteLine($"\t{Language.CurrentLanguage.Get("VERINFOSTR01", "Target Config:")}\t" + Repo.CurrentConfigVer.ToString() + $"({Repo.CurrentConfigVer.Build})");
            Console.WriteLine($"\t{Language.CurrentLanguage.Get("VERINFOSTR02", "Min Config:")}\t" + Repo.MinConfigVer.ToString() + $"({Repo.MinConfigVer.Build})");
        }

        static void ShowHelp()
        {
            for (int i = 0; i < 19; i++)
            {
                Console.WriteLine(Language.CurrentLanguage.Get($"HELP{i:D2}"));
            }
        }
        public static void ShowOldHelp()
        {

            Console.WriteLine("extgit [Options|Operations]");
            Console.WriteLine("Operations:");
            Console.WriteLine("\t-C|COMMIT\tCommit changes to ExtGit workload.");
            Console.WriteLine("\t-O|CHECKOUT\tCheck out newest workload and overwrite current files.");
            Console.WriteLine("\t-H|--H|-?|\tShow this help.");
            Console.WriteLine("\t--?|HELP|");
            Console.WriteLine("\t-HELP|--HELP");
            Console.WriteLine("\t-V|--V|VER|\tShow version information");
            Console.WriteLine("\tVERSION|-VERSION|");
            Console.WriteLine("\t-VER|--VER|");
        }
    }
    enum Operation
    {
        Commit, Checkout, None, Help, Version, Create, Graft, FunctionTest
    }
}
