using ExtGit.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExtGit.Core.Version1
{
    public class Repo
    {
        private string RepoPath;

        public static readonly Version CurrentConfigVer = new Version(1, 0, 1, 0);
        public static readonly Version MinConfigVer = new Version(1, 0, 1, 0);
        public Version ExtGitVer;
        public Version ExtGitVerCore;
        public int MaxFileSize = 1024 * 1024 * 99;//100MB is the max file size limit of GitHub.
        public bool AutoTrack = true;//Auto track files that overflow the limit.
        public double TraceTriggerLevel = 0.9;//MaxFile*TraceTriggerLevel is the actual detection-line.
        public bool IgnoreIgnoredFile = true;//Load .gitignore
        public List<string> AutoTraceFileTypes = new List<string>();//Although auto trace is enough, still provide a way to automatically trace files on file type.
        public List<string> AutoTraceFilePaths = new List<string>();//Although auto trace is enough, still provide a way to automatically trace files on file type.
        public List<string> IgnoredFilePaths = new List<string>();//Besides .gitignore, still can ignore some files incase that some auto-generated files will never reach the real limit but reaches the detection line.

        private List<TraceIndex> TracedFiles = new List<TraceIndex>();

        FileInfo ConfigurationFile;

        bool isTemplate = false;
        public Repo()
        {
            isTemplate = true;
        }
        public Repo(string RepoPath)
        {
            this.RepoPath = RepoPath;
            ConfigurationFile = new FileInfo(Path.Combine(RepoPath, ".extgit", ".extgitconf", "Repo.extgit"));
            if (!ConfigurationFile.Exists)
            {
                throw new Exception(Language.CurrentLanguage.Get("ERROR_CODE00", "Target directory is not an ExtGit directory!"));
            }
            Load();
            DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(RepoPath, ".extgit", ".extgitconf", "Traces"));
            var existedTraces = directoryInfo.EnumerateFiles();
            foreach (var item in existedTraces)
            {
                TracedFiles.Add(new TraceIndex(item.FullName));
            }
        }
        public static void Create(Repo r, string RepoPath)
        {
            var ConfigurationFile = new FileInfo(Path.Combine(RepoPath, ".extgit", ".extgitconf", "Repo.extgit"));

        }
        void Load()
        {
            var content = File.ReadAllLines(ConfigurationFile.FullName);
            foreach (var item in content)
            {
                if (item.StartsWith("ExtGitVer="))
                {
                    ExtGitVer=Version.Parse(item.Substring("ExtGitVer=".Length));
                }
                else if (item.StartsWith("ExtGitCoreVer="))
                {
                    ExtGitVerCore = Version.Parse(item.Substring("ExtGitCoreVer=".Length));
                }
                else if (item.StartsWith("MaxFileSize="))
                {
                    MaxFileSize = int.Parse(item.Substring("MaxFileSize=".Length));
                }
                else if (item.StartsWith("AutoTrack="))
                {
                    AutoTrack = bool.Parse(item.Substring("AutoTrack=".Length));
                }
            }
        }
        public void Commit(ref double progress)
        {
            if (isTemplate == true)
            {
                throw new Exception(Language.CurrentLanguage.Get("ERROR_CODE01", "Current repository is a TEMPLATE repository!"));
            }
            FileInfo GitIgnorance = new FileInfo(Path.Combine(RepoPath, ".gitignore"));
            var ignoranceF = File.ReadAllLines(GitIgnorance.FullName);

        }
        //public List<>
        public void Checkout(ref double progress)
        {
            if (isTemplate == true)
            {
                throw new Exception(Language.CurrentLanguage.Get("ERROR_CODE01", "Current repository is a TEMPLATE repository!"));
            }
        }
    }
}
