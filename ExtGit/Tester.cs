using ExtGit.Core.FileHash;
using System;
using System.Collections.Generic;
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
            }
        }
    }
}
