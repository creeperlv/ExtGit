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
        public static void CopyPath(DirectoryInfo Origin, DirectoryInfo Target,params string[] ignorances)
        {
            foreach (var item in ignorances)
            {
                if (Origin.FullName.ToUpper().StartsWith(item.ToUpper()))
                {
                    return;                
                }
            }
            if (!Target.Exists)
                Target.Create();
            foreach (var item in Origin.GetDirectories())
            {

                CopyPath(item, new DirectoryInfo(Path.Combine(Target.FullName, item.Name)));
            }
            foreach (var item in Origin.GetFiles())
            {
                item.CopyTo(Path.Combine(Target.FullName, item.Name),true);
            }
            return;
        }
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
