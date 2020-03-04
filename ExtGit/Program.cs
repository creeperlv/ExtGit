using ExtGit.Core;
using ExtGit.Core.Version1;
using ExtGit.Localization;
using System;
using System.IO;

namespace ExtGit
{
    class Program
    {
        static Operation CurrentOperation= Operation.None;
        static Version ShellVersion = new Version(1, 0, 1, 0);
        static void Main(string[] args)
        {
            /**
             * 
             * Layout:
             *      Repo/
             *      Repo/<WorkLoad>
             *      Repo/.ExtGit/
             *      Repo/.ExtGit/<ExtGitWorkLoad>
             *      Repo/.ExtGit/.extgitconfigs/ - ExtGit Configurations
             *      Repo/.ExtGit/.git/...
             * 
             **/
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToUpper())
                {
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
                    case "-VER":
                    case "--VER":
                        CurrentOperation = Operation.Version;
                        break;
                    default:
                        break;
                }
            }
            Console.WriteLine(Language.CurrentLanguage.Get("TITLE0","ExtGit - Designed for commit large file without GIT-LFS to host platforms."));
            Console.WriteLine(Language.CurrentLanguage.Get("PREVIEW00","This software still in an early preview"));
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
                            Console.WriteLine(e.Message);
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
                            Console.Write("Fatal:");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(e.Message);
                        }
                    }
                    break;
                case Operation.None:
                case Operation.Help:
                    ShowHelp();
                    break;
                case Operation.Version:
                    ShowVersion();
                    break;
                default:
                    break;
            }
        }

        private static void ShowVersion()
        {
            Console.WriteLine("");
            Console.WriteLine("ExtGit Components:");
            Console.WriteLine("");
            Console.WriteLine("\tShell Version:\t" + ShellVersion.ToString()+$"({ShellVersion.Build})");
            Console.WriteLine("\tCore Version:\t" + CoreLib.CurrentCore.CoreVersion.ToString() + $"({CoreLib.CurrentCore.CoreVersion.Build})");
            Console.WriteLine("");
            Console.WriteLine("\tTarget Config:\t" + Repo.CurrentConfigVer.ToString() + $"({Repo.CurrentConfigVer.Build})");
            Console.WriteLine("\tMin Config:\t" + Repo.MinConfigVer.ToString() + $"({Repo.MinConfigVer.Build})");
        }

        static void ShowHelp()
        {
            Console.WriteLine("extgit [Options|Operations]");
            Console.WriteLine("Operations:");
            Console.WriteLine("\t-C|COMMIT\tCommit changes to ExtGit workload.");
            Console.WriteLine("\t-O|CHECKOUT\tCheck out newest workload and overwrite current files.");
            Console.WriteLine("\t-H|--H|-?|\tShow this help.");
            Console.WriteLine("\t--?|HELP|");
            Console.WriteLine("\t-HELP|--HELP");
        }
    }
    enum Operation
    {
        Commit,Checkout,None,Help,Version
    }
}
