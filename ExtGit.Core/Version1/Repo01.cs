using System;
using System.Collections.Generic;
using System.Text;

namespace ExtGit.Core.Version1
{
    public partial class Repo
    {
        public static void Graft(string GitRepoLoation,GraftOptions options)
        {

        }
    }
    public class GraftOptions
    {
        public bool KeepGitHistory = true;
        public bool CommitAfterGraft = true;
    }
}
