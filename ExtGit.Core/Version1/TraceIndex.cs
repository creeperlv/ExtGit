using ExtGit.Core.FileHash;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExtGit.Core.Version1
{
    public class TraceIndex
    {
        public string RelativeFilePath;
        public Dictionary<int, string> Chunks = new Dictionary<int, string>();
        public string FileHash;
        public string IndexFilePath;
        private string RepoBase;
        TraceIndex()
        {

        }
        public TraceIndex(FileInfo IndexFile,string RepoBaseDir)
        {
            IndexFilePath = IndexFile.FullName;
            RepoBase = RepoBaseDir;
        }
        public static TraceIndex Track(FileInfo TargetFile, Repo ParentRepo)
        {
            TraceIndex traceIndex = new TraceIndex();

            {
                traceIndex.IndexFilePath = TargetFile.FullName.Substring(ParentRepo.RepoPath.Length);
                traceIndex.FileHash = SHA256.ComputeSHA256(TargetFile.FullName);
            }

            return traceIndex;
        }
        public void CombineAndOverwrite()
        {

        }
        public void DifferAndUpdate()
        {
            var CurrentHash= SHA256.ComputeSHA256(Path.Combine(RepoBase, RelativeFilePath));
            if (CurrentHash == FileHash)
            {
                //The file can be considered as 'same'.
                return;
            }
            else
            {
                FileInfo f = new FileInfo(Path.Combine(RepoBase, RelativeFilePath));
                int ChunkID = 0;
                for (int i = 0; i < f.Length; i++)
                {
                    if (Chunks.ContainsKey(ChunkID))
                    {

                    }
                    else
                    {
                        //Create Chunk.
                    }
                }
            }
        }
    }
}
