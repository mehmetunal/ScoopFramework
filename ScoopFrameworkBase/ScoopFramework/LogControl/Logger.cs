using System;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using ScoopFramework.DataConnection;

namespace ScoopFramework.LogControl
{
    public static class Logger
    {
        static string FILE = "";
        static object sync = new object();
        private static void WriteTime()
        {
            WriteTime(false);
        }
        private static void WriteTime(bool errorconsole)
        {
            lock (sync)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    if (errorconsole)
                    {
                        Console.Error.Write(DateTime.Now.ToString().PadRight(20) + " ");
                    }
                    else
                    {
                        Console.Write(DateTime.Now.ToString().PadRight(20) + " ");
                    }
                    Console.ResetColor();
                }
                catch
                {
                }
            }
        }
        public static void Log(string mesaj, bool success, bool dbLog)
        {
            if (!success) return;
            if (dbLog)
            {
                var sql = "Insert into LogDB created,Mesaj VALUES (" + DateTime.Now + "," + mesaj + ")";
                using (var command = new SqlCommand(sql, DBHelper.Connection()))
                {
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                lock (sync)
                {
                    WriteTime(true);
                    try
                    {
                        Save(string.Format(mesaj));
                    }
                    catch
                    {
                    }
                }
            }
        }
        private static void Save(string str)
        {

            if (string.IsNullOrEmpty(str)) { return; }

            var path = HttpContext.Current.Server.MapPath("\\log");

            if (string.IsNullOrEmpty(FILE))
            {
                FILE = string.Format("{0}\\{1}-SYSTEM-LOG.txt", path, DateTime.Now.ToString("yyyy-MM-dd"));
            }

            if (!File.Exists(FILE))
            {
                System.IO.File.WriteAllText(FILE, DateTime.Now.ToString());
            }

            using (StreamWriter sw = File.AppendText(FILE))
            {
                sw.WriteLine(DateTime.Now + "        " + str);
            }
        }

    }
}
