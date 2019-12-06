using System;
using System.IO;

namespace ScoopFramework.TTGenerators.Helper
{
    public class FileHelper
    {
        public static void ForceCreateFile(string path)
        {
            var file = Path.GetFileName(path);
            if (file == null)
                throw new Exception("Dosya adı belirtilmedi.");


            var folder = Path.GetDirectoryName(path);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            using (File.Create(path)) { }
        }

        public static string[] DirectoryGetFiles(string path, string searchPattern)
        {
            if (!Directory.Exists(path))
                return new string[0];
            return Directory.GetFiles(path, searchPattern);
        }
    }
}
