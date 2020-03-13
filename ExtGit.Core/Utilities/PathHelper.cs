using System;
using System.IO;
using System.Web;

namespace ExtGit.Core.Utilities
{
    /// <summary>
    /// Tools on path, including compute
    /// </summary>
    public class PathHelper
    {
        public static string GetRelativePath(string master, string slave)
        {
            if (!master.EndsWith(Path.DirectorySeparatorChar + ""))
            {
                master += Path.DirectorySeparatorChar;
            }
            Uri u1 = new Uri(master, UriKind.Absolute);
            Uri u2 = new Uri(slave, UriKind.Absolute);
            Uri u3 = u1.MakeRelativeUri(u2);

            return HttpUtility.UrlDecode(u3.OriginalString);
        }
        public static void RemoveFolderR(DirectoryInfo directory)
        {
            foreach (var item in directory.EnumerateDirectories())
            {
                RemoveFolderR(item);
            }
            foreach (var item in directory.EnumerateFiles())
            {
                item.Delete();
            }
            directory.Delete();
        }
    }
}
