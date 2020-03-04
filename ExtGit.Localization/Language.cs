using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExtGit.Localization
{
    public class Language
    {
        public Dictionary<string, string> LanguagePair = new Dictionary<string, string>();
        public static Language CurrentLanguage = new Language();
        public Language()
        {
            try
            {
                var LN = System.Globalization.CultureInfo.CurrentCulture.Name;
                FileInfo f = new FileInfo(typeof(Language).Assembly.Location);
                var LD = new DirectoryInfo(Path.Combine(f.Directory.FullName, "Locale", LN));
                if (!LD.Exists)
                {
                    LD = new DirectoryInfo(Path.Combine(f.Directory.FullName, "Locale", "en-US"));
                }
                foreach (var item in LD.EnumerateFiles())
                {
                    var contents = File.ReadAllLines(item.FullName);
                    foreach (var line in contents)
                    {
                        if (line.IndexOf("=") > 0)
                        {
                            var key = line.Substring(0, line.IndexOf("="));
                            var content = line.Substring(line.IndexOf("=") + 1).Replace("\\r", "\r");
                            LanguagePair.Add(key, content);
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Missing locale file.");
            }
        }
        public string Get(string key, string fallback = "<MISSING>")
        {
            return LanguagePair.ContainsKey(key) ? LanguagePair[key] : fallback;
        }
    }
}
