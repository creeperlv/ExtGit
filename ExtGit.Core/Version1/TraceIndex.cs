﻿using ExtGit.Core.FileHash;
using ExtGit.Core.Utilities;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExtGit.Core.Version1
{
    public class TraceIndex
    {
        public static readonly Version TraceVersion = new Version(1, 0, 0, 0);
        public string RelativeFilePath;
        public Dictionary<int, string> Chunks = new Dictionary<int, string>();
        public string FileHash;
        public FileInfo IndexFile;
        public bool Compressed = false;
        //private string RepoBase;
        private bool isDeletionScheduled = false;
        private Repo Parent;
        TraceIndex()
        {

        }
        public TraceIndex(FileInfo IndexFile, Repo RepoBase)
        {
            this.IndexFile = IndexFile;
            Parent = RepoBase;
            LoadIndex();
            Debugger.CurrentDebugger.Log("Trace Index Found:" + IndexFile.Name + $", FileHash:{FileHash}", LogLevel.Development);
        }
        public void Schedule()
        {
            isDeletionScheduled = true;
        }
        public void Redress()
        {
            isDeletionScheduled = false;
        }
        public void Urgent()
        {
            if (isDeletionScheduled)
                IndexFile.Delete();
        }
        public static TraceIndex Track(FileInfo TargetFile, Repo ParentRepo)
        {
            TraceIndex traceIndex = new TraceIndex();

            {
                traceIndex.RelativeFilePath = PathHelper.GetRelativePath(ParentRepo.RepoPath, TargetFile.FullName);
                traceIndex.FileHash = "INIT";
                //traceIndex.FileHash = SHA256Hash.ComputeSHA256(TargetFile.FullName);
            }
            traceIndex.Parent = ParentRepo;
            return traceIndex;
        }
        public void LoadIndex()
        {
            var lns = File.ReadAllLines(IndexFile.FullName);
            foreach (var item in lns)
            {
                if (item.StartsWith("FileHash="))
                {
                    string hash = item.Substring("FileHash=".Length);
                }
                else if (item.StartsWith("RelateiveFilePath="))
                {
                    RelativeFilePath = item.Substring("RelateiveFilePath=".Length);
                }
                else if (item.StartsWith("Compressed="))
                {
                    Compressed = bool.Parse(item.Substring("Compressed=".Length));
                }
                else if (item.StartsWith("Chunck."))
                {
                    var temp01 = item.Substring("Chunck.".Length);
                    var Num = temp01.Substring(0, temp01.IndexOf('='));
                    var FileName = item.Substring(item.IndexOf("=") + 1);
                    Chunks.Add(int.Parse(Num), FileName);
                }
            }
        }
        public static TraceIndex TrackAndRecord(FileInfo TargetFile, Repo ParentRepo)
        {
            var T = Track(TargetFile, ParentRepo);
            var RandomName = Guid.NewGuid() + ".extgit-trace";
            var FileName = Path.Combine(ParentRepo.RepoPath, ".extgit", ".extgitconf", "Traces", RandomName);
            while (File.Exists(FileName) == true)
            {
                //If it happens that the generated guid is used, generate a new one.
                RandomName = Guid.NewGuid() + ".extgit-trace";
                FileName = Path.Combine(ParentRepo.RepoPath, ".extgit", ".extgitconf", "Traces", RandomName);
            }
            File.Create(FileName).Close();

            {
                //Save index.
                var NewContent = "#Generated by ExtGit.Core\r\n";
                NewContent += $"TraceVersion={TraceVersion}\r\n";
                NewContent += $"RelateiveFilePath={T.RelativeFilePath}\r\n";
                NewContent += $"FileHash={T.FileHash}\r\n";
                foreach (var item in T.Chunks)
                {
                    NewContent += $"Chunck.{item.Key}={item.Value}";
                }
                File.WriteAllText(FileName, NewContent);
            }

            Debugger.CurrentDebugger.Log("Start to trace:" + T.RelativeFilePath, Utilities.LogLevel.Development);
            T.IndexFile = new FileInfo(FileName);
            return T;
        }

        public string PreProcess_Overwrite()
        {

            if (Compressed == true || Features.CompressLargeFiles == true)
            {
                {
                    var chunk = Path.Combine(Parent.RepoPath, ".extgit", ".extgitconf", "Traces", Path.GetFileNameWithoutExtension(IndexFile.FullName));
                    var Entry = Path.Combine(chunk, "CompressedFile");
                    //Compress, then delete.
                    return Entry;
                }
            }
            else
                return Path.Combine(Parent.RepoPath, RelativeFilePath);
        }
        public void PostProcess_Overwrite()
        {

            if (Compressed == true || Features.CompressLargeFiles == true)
            {
                {
                    var chunk = Path.Combine(Parent.RepoPath, ".extgit", ".extgitconf", "Traces", Path.GetFileNameWithoutExtension(IndexFile.FullName));
                    var Entry = Path.Combine(chunk, "CompressedFile");
                    var Target = Path.Combine(Parent.RepoPath, RelativeFilePath);
                    byte[] dataBuffer = new byte[4096];

                    using (var fs = new FileStream(Entry, FileMode.Open, FileAccess.Read))
                    {
                        using (GZipInputStream gzipStream = new GZipInputStream(fs))
                        {

                            using (FileStream fsOut = File.Create(Target))
                            {
                                StreamUtils.Copy(gzipStream, fsOut, dataBuffer);
                            }
                        }
                    }
                    File.Delete(Target);
                }
            }
        }

        public string PreProcess_Differ()
        {
            if (Compressed == true || Features.CompressLargeFiles == true)
            {
                {
                    var oriFile = Path.Combine(Parent.RepoPath, RelativeFilePath);
                    var chunk = Path.Combine(Parent.RepoPath, ".extgit", ".extgitconf", "Traces", Path.GetFileNameWithoutExtension(IndexFile.FullName));
                    var Entry = Path.Combine(chunk, "CompressedFile");
                    Stream outStream = File.Create(Entry);
                    Stream gzoStream = new GZipOutputStream(outStream);
                    byte[] DataBuffer = new byte[4096];
                    using (var FS = File.Open(oriFile, FileMode.Open))
                    {

                        StreamUtils.Copy(FS, gzoStream, DataBuffer);
                    }
                    gzoStream.Close();
                    Compressed = true;
                    //Compress, then delete.
                    return Entry;
                }
            }
            else
                return Path.Combine(Parent.RepoPath, RelativeFilePath);


        }
        public void PostProcess_Differ()
        {
            if (Compressed == true || Features.CompressLargeFiles == true)
            {
                var chunk = Path.Combine(Parent.RepoPath, ".extgit", ".extgitconf", "Traces", Path.GetFileNameWithoutExtension(IndexFile.FullName));
                var Entry = Path.Combine(chunk, "CompressedFile");
                File.Delete(Entry);
            }

        }

        //Work with check out.
        public void CombineAndOverwrite()
        {
            var CurrentHash = SHA256Hash.ComputeSHA256(Path.Combine(Parent.RepoPath, RelativeFilePath));
            if (CurrentHash == FileHash)
            {
                //The file can be considered as 'same'.
                return;
            }
            else
            {
                var p = PreProcess_Overwrite();
                if (!File.Exists(p))
                {
                    File.Create(p).Close();
                }
                var FW = File.OpenWrite(p);
                for (int i = 0; i < Chunks.Count; i++)
                {
                    var f = Chunks[i];
                    var chunk = Path.Combine(Parent.RepoPath, ".extgit", ".extgitconf", "Traces", Path.GetFileNameWithoutExtension(IndexFile.FullName), f);
                    var FR = File.OpenRead(chunk);
                    byte[] CHUNK = new byte[128 * 1024];
                    int RL = 0;
                    while ((RL = FR.Read(CHUNK, 0, CHUNK.Length)) != 0)
                    {
                        FW.Write(CHUNK, 0, RL);
                        FW.Flush();
                    }
                    FR.Close();
                }
                FW.Close();
                PostProcess_Overwrite();
            }
        }
        //Work with Commit
        public void DifferAndUpdate()
        {
            if (isDeletionScheduled)
            {
                //When deletion is scheduled, delete all related chunks.
                var chunk = Path.Combine(Parent.RepoPath, ".extgit", ".extgitconf", "Traces", Path.GetFileNameWithoutExtension(IndexFile.FullName));
                if (Directory.Exists(chunk))
                {
                    DirectoryInfo d = new DirectoryInfo(chunk);

                    foreach (var item in d.EnumerateFiles())
                    {
                        item.Delete();
                    }
                    d.Delete();
                }
                return;
            }
            var CurrentHash = SHA256Hash.ComputeSHA256(Path.Combine(Parent.RepoPath, RelativeFilePath));
            if (CurrentHash == FileHash)
            {
                //The file can be considered as 'same'.
                return;
            }
            else
            {

                FileInfo f = new FileInfo(PreProcess_Differ());
                int ChunkID = 0;
                long ChunkMaxLength = Parent.MaxFileSize;
                var FR = f.OpenRead();
                byte[] ByteBlock = new byte[1024];//1KB.
                {

                    var chunk = Path.Combine(Parent.RepoPath, ".extgit", ".extgitconf", "Traces", Path.GetFileNameWithoutExtension(IndexFile.FullName));
                    Directory.CreateDirectory(chunk);
                }
                for (long i = 0; i < f.Length;)
                {
                    long Position = 0;
                    if (Chunks.ContainsKey(ChunkID))
                    {
                        int AP = 0;
                        //Locate Chunk File;
                        var FN = Chunks[ChunkID];
                        var TN = Path.GetFileNameWithoutExtension(IndexFile.Name);
                        var chunk = Path.Combine(Parent.RepoPath, ".extgit", ".extgitconf", "Traces", Path.GetFileNameWithoutExtension(IndexFile.FullName), FN);
                        Debugger.CurrentDebugger.Log("Chunk find:" + chunk);
                        File.Delete(chunk);//Clear chunk before overwrite it.
                        var FW = File.Create(chunk);
                        
                        while ((AP = FR.Read(ByteBlock, 0, ByteBlock.Length)) != 0)
                        {
                            {
                                FW.Write(ByteBlock, 0, ByteBlock.Length);
                                FW.Flush();
                                //Write To File
                            }
                            Position += AP;
                            if (Position >= Parent.MaxFileSize)
                            {
                                //i += Position;
                                break;
                            }
                        }
                        ChunkID++;
                        FW.Close();
                    }
                    else
                    {
                        //Create Chunk.

                        int AP = 0;
                        //Locate Chunk File;
                        var FN = Guid.NewGuid() + ".Data-Chunk";
                        var TN = Path.GetFileNameWithoutExtension(IndexFile.Name);
                        var chunk = Path.Combine(Parent.RepoPath, ".extgit", ".extgitconf", "Traces", Path.GetFileNameWithoutExtension(IndexFile.FullName), FN);
                        
                        Debugger.CurrentDebugger.Log("Chunk created:" + chunk, Utilities.LogLevel.Development);
                        var FW = File.Create(chunk);
                        while ((AP = FR.Read(ByteBlock, 0, ByteBlock.Length)) != 0)
                        {
                            {
                                FW.Write(ByteBlock, 0, ByteBlock.Length);
                                FW.Flush();
                                //Write To File
                            }
                            Position += AP;
                            if (Position >= Parent.MaxFileSize)
                            {
                                break;
                            }
                        }
                        FW.Close();
                        //When Chunk is written, the chunk will be add into index file.
                        Chunks.Add(ChunkID, FN);
                        ChunkID++;
                    }

                    i += Position;
                }
                {
                    //Process when ths file is smaller than before.
                    //ChunkID is 1 larger than real last ID, so it can mean the count in the same time.
                    if (Chunks.Count > ChunkID)
                    {
                        for (int i = ChunkID; i < Chunks.Count; i++)
                        {
                            var FN = Chunks[i];
                            var chunk = Path.Combine(Parent.RepoPath, ".extgit", ".extgitconf", "Traces", Path.GetFileNameWithoutExtension(IndexFile.FullName), FN);
                            File.Delete(chunk);
                            Chunks.Remove(i);
                        }
                    }
                }


                FR.Close();
                //Finalize, only when the file is different, index file will be updated.
                {
                    //Save index.
                    var NewContent = "#Generated by ExtGit.Core\r\n";
                    NewContent += $"TraceVersion={TraceVersion}\r\n";
                    NewContent += $"RelateiveFilePath={RelativeFilePath}\r\n";
                    NewContent += $"FileHash={CurrentHash}\r\n";
                    NewContent += $"Compressed={Compressed}\r\n";
                    foreach (var item in Chunks)
                    {
                        NewContent += $"Chunck.{item.Key}={item.Value}\r\n";
                    }
                    File.WriteAllText(IndexFile.FullName, NewContent);
                }
                PostProcess_Differ();
            }
        }
    }
}
