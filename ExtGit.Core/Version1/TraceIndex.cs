﻿using ExtGit.Core.FileHash;
using ExtGit.Core.Utilities;
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
        //private string RepoBase;
        private Repo Parent;
        TraceIndex()
        {

        }
        public TraceIndex(FileInfo IndexFile, Repo RepoBase)
        {
            IndexFilePath = IndexFile.FullName;
            Parent = RepoBase;
        }
        public static TraceIndex Track(FileInfo TargetFile, Repo ParentRepo)
        {
            TraceIndex traceIndex = new TraceIndex();

            {
                traceIndex.RelativeFilePath = PathHelper.GetRelativePath(ParentRepo.RepoPath, TargetFile.FullName);
                traceIndex.FileHash = SHA256.ComputeSHA256(TargetFile.FullName);
            }

            return traceIndex;
        }
        public void CombineAndOverwrite()
        {
            var CurrentHash = SHA256.ComputeSHA256(Path.Combine(Parent.RepoPath, RelativeFilePath));
            if (CurrentHash == FileHash)
            {
                //The file can be considered as 'same'.
                return;
            }
            else
            {
                var p = Path.Combine(Parent.RepoPath, RelativeFilePath);
                var FW = File.OpenWrite(p);
                for (int i = 0; i < Chunks.Count; i++)
                {
                    var f = Chunks[i];
                    var chunk = Path.Combine(Parent.RepoPath, ".extgit", ".extgitconf", "Traces", Path.GetFileNameWithoutExtension(IndexFilePath), f);
                    var FR = File.OpenRead(chunk);
                    byte[] CHUNK = new byte[128*1024];
                    int RL = 0;
                    while ((RL=FR.Read(CHUNK,0,CHUNK.Length))!=0)
                    {
                        FW.Write(CHUNK,0,RL);
                        FW.Flush();
                    }
                    FR.Close();
                }
                FW.Close();
            }
        }
        public void DifferAndUpdate()
        {
            var CurrentHash = SHA256.ComputeSHA256(Path.Combine(Parent.RepoPath, RelativeFilePath));
            if (CurrentHash == FileHash)
            {
                //The file can be considered as 'same'.
                return;
            }
            else
            {
                FileInfo f = new FileInfo(Path.Combine(Parent.RepoPath, RelativeFilePath));
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
