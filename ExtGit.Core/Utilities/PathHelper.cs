using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExtGit.Core.Utilities
{
    public class PathHelper
    {
        public static string GetRelativePath(string master, string slave)
        {
            if (!master.EndsWith(Path.DirectorySeparatorChar+""))
            {
                master += Path.DirectorySeparatorChar;
            }
            Uri u1 = new Uri(master, UriKind.Absolute);
            Uri u2 = new Uri(slave, UriKind.Absolute);
            Uri u3 = u1.MakeRelativeUri(u2);
            return u3.OriginalString;
        }
    }
}
