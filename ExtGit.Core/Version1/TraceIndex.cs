using System;
using System.Collections.Generic;
using System.Text;

namespace ExtGit.Core.Version1
{
    public class TraceIndex
    {
        public string RelativeFilePath;
        public Dictionary<int, string> Chunks = new Dictionary<int, string>();

        public string IndexFilePath;
        public TraceIndex(string IndexFilePath)
        {

        }
        public void CombineAndOverwrite()
        {
        
        }
        public void DifferAndUpdate()
        {
        
        }
    }
}
