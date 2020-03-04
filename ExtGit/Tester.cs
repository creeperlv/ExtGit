using ExtGit.Core.FileHash;
using ExtGit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExtGit
{
    public class Tester
    {
        public static void Test00()
        {
            {
                Console.WriteLine("");
                Console.WriteLine("Test Field");
                Console.WriteLine("");
                try
                {
                    Console.WriteLine("\tTest ID: {04EE0BD8-57E6-4013-B972-BFE4FB081235}");
                    Console.WriteLine("\t\tFile Hash:" + SHA256.ComputeSHA256("./ExtGit.exe"));
                    Console.WriteLine("\t\t[OK]");
                }
                catch (Exception e)
                {
                    Console.WriteLine("\tError:" + e);
                }
                try
                {
                    Console.WriteLine("\tTest ID: {CA51CA71-DAEF-45F2-A3EA-6445DAA8A950}");
                    var master = (new DirectoryInfo("./")).Parent.FullName;
                    var slave= (new FileInfo("./ExtGit.dll")).FullName;
                    Console.WriteLine("\t\tRelative Path:" + PathHelper.GetRelativePath(master,slave));
                    Console.WriteLine("\t\tAbsolute Path(Master):" + master);
                    Console.WriteLine("\t\tAbsolute Path(Slave):" + slave);
                    Console.WriteLine("\t\t[OK]");
                }
                catch (Exception e)
                {
                    Console.WriteLine("\tError:" + e);
                }
            }
        }
    }
}
