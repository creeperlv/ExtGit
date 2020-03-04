using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExtGit.Core.FileHash
{
    public class SHA256
    {
        public static string ComputeSHA256(string fileName)
        {
            string _Result = "";
            if (File.Exists(fileName))
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    var calculator = System.Security.Cryptography.SHA256.Create();
                    byte[] buffer = calculator.ComputeHash(fs);
                    calculator.Clear();
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        stringBuilder.Append(buffer[i].ToString("x2"));
                    }
                    _Result = stringBuilder.ToString();
                }
            }
            return _Result.ToUpper();
        }
    }
}
