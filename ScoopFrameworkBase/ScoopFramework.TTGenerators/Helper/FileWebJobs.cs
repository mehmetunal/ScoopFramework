using System;
using System.Web;

namespace ScoopFramework.TTGenerators.Helper
{
    public class FileWebJobs
    {
        private static string GetFileUpload(HttpContext context, string _filename)
        {
            HttpFileCollection files = context.Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile file = files[i];
                file.SaveAs(@"D:\" + file.FileName.ToString());
                _filename += "[" + file.FileName + "]";
            }
            return _filename;
        }
      
        public static string FileBoyut(long bytes)
        {
            const int scale = 1024;
            string[] orders = new string[] { "TB","GB", "MB", "KB", "Bytes" };
            long max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (string order in orders)
            {
                if (bytes > max)
                    return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);

                max /= scale;
            }
            return "0 Bytes";
        }
    }
}
