using ExtGit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExtGit.Core.Version1
{
    public partial class Repo
    {
        public static void Graft(string GitRepoLoation,GraftOptions options,Version ShellVersion)
        {
            var baseGit = new DirectoryInfo(GitRepoLoation);
            if (options.KeepGitHistory)
            {
                if(options.KeepOriginalGitRepo==false)
                Directory.Move(Path.Combine(baseGit.FullName, ".git"), Path.Combine(baseGit.FullName, ".extgit", ".git"));
                else
                {
                    PathHelper.CopyPath(new DirectoryInfo(Path.Combine(baseGit.FullName, ".git")), new DirectoryInfo(Path.Combine(baseGit.FullName, ".extgit", ".git")), Path.Combine(baseGit.FullName, ".extgit"));
                }
            }
            else
            {
                Repo repo = new Repo();
                repo.ExtGitVer = ShellVersion;
                repo.ExtGitVerCore = CoreLib.CurrentCore.CoreVersion;
                Repo.Create(repo, GitRepoLoation);
                File.Copy(Path.Combine(baseGit.FullName, ".git", "config"), Path.Combine(baseGit.FullName, ".extgit", ".git", "config"));
            }

            File.Copy(Path.Combine(baseGit.FullName, ".gitignore"), Path.Combine(baseGit.FullName, ".extgit", ".gitignore"));
            if (options.CommitAfterGraft)
            {
                Repo r = new Repo(GitRepoLoation);
                double a=0;
                r.Commit(ref a);
            }
        }
    }
    public class GraftOptions
    {
        public bool KeepGitHistory = true;
        public bool KeepOriginalGitRepo = false;
        public bool CommitAfterGraft = true;
    }
}
