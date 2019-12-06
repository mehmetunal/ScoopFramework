using System;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using ScoopFramework.Model;

namespace ScoopFramework.Helper
{
    public class DBScripter
    {
        private Server server = null;
        public Scripter scripter = null;
        private SqlConnection sqlConnection = null;
        private string connectionString = String.Empty;
        private ServerConnection serverConnection = null;

        public DBScripter(string ConnectionString)
        {
            this.connectionString = ConnectionString;
            this.sqlConnection = new SqlConnection(ConnectionString);
            this.serverConnection = new ServerConnection(this.sqlConnection);
            this.server = new Server(this.serverConnection);
            this.scripter = new Scripter(this.server)
            {
                Options = {
                    AnsiFile = true,
                    ContinueScriptingOnError = true,
                    ScriptOwner = true,
                    ScriptSchema = true,
                    Statistics = true,
                    ScriptData = true,
                }
            };

        }

        public string[] GetTableNames()
        {
            return server.Databases[this.sqlConnection.Database].Tables.Cast<Table>().Select(a => a.Name).ToArray();
        }

        public string GetTableScript(string Table)
        {
            try
            {
                var res = new StringBuilder();
                var table = server.Databases[this.sqlConnection.Database].Tables.Cast<Table>().Where(a => a.Name == Table && !a.IsSystemObject).FirstOrDefault();

                scripter = new Scripter(server) { Options = { ScriptData = false } };
                var script = scripter.EnumScript(new SqlSmoObject[] { table });
                foreach (var line in script)
                    res.AppendLine(line);

                return res.ToString();

            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }

        }

        public ReturnValue GenerateTableScript(string[] tableNames = null)
        {

            try
            {

                if (tableNames == null)
                    tableNames = this.GetTableNames();

                var time = String.Format("{0:dd_MM_yyyy_HHmm}", DateTime.Now);
                var path = HttpContext.Current.Server.MapPath(@"\App_Data");

                if (!Directory.Exists(path + @"\" + time))
                    Directory.CreateDirectory(path + @"\" + time);
                else
                    return new ReturnValue { Success = false, Mesaj = time + " Zamanı için zaten bir dosya yaratıldı." };

                var tables = server.Databases[this.sqlConnection.Database].Tables.Cast<Table>()
                    .Where(a => tableNames.ToList().FindIndex(b => b.Equals(a.Name, StringComparison.OrdinalIgnoreCase)) != -1 && !a.IsSystemObject);

                foreach (var table in tables)
                {
                    var res = new StringBuilder();
                    var file = String.Format(@"{0}\{1}\{2}.sql", path, time, table.Name);

                    if (File.Exists(file))
                        continue;

                    var tableName = "[" + table.Schema + "].[" + table.Name + "]";
                    res.AppendLine("IF OBJECT_ID('" + tableName + "', 'U') IS NOT NULL DROP TABLE " + tableName + ";");

                    foreach (var line in scripter.EnumScript(new SqlSmoObject[] { table }))
                        res.AppendLine(line);

                    File.WriteAllText(file, res.ToString());

                }

                ZipFile.CreateFromDirectory(path + @"\" + time, path + @"\" + time + ".zip", CompressionLevel.Optimal, false);

                return new ReturnValue { Success = true, Data = path + @"\" + time + ".zip" };

            }
            catch (System.Exception ex)
            {
                return new ReturnValue { Success = false, Mesaj = ex.ToString() };
            }

        }

        public ReturnValue ExecuteSQLFile(string fileDir)
        {

            var files = Directory.GetFiles(fileDir).Where(a => a.EndsWith(".sql")).ToList();
            var executedFiles = 0;

            foreach (var file in files)
            {

                try
                {
                    var _file = new FileInfo(file);

                    SqlConnection conn = new SqlConnection(this.connectionString);
                    Server server = new Server(new ServerConnection(conn));

                    using (var sr = new StreamReader(file))
                    {

                        var say = 0;
                        var script = new StringBuilder();

                        while (sr.Peek() >= 0)
                        {

                            say++;

                            script.AppendLine(sr.ReadLine());

                            if (say % 5000 == 0)
                            {

                                var res = server.ConnectionContext.ExecuteNonQuery(script.ToString());
                                script.Clear();

                            }

                        }

                        var resLast = server.ConnectionContext.ExecuteNonQuery(script.ToString());

                    }
                    executedFiles++;
                }
                catch (System.Exception)
                { }

            }

            return new ReturnValue() { Success = true, Mesaj = executedFiles + " / " + files.Count() + " Başarı ile işlendi." };

        }

    }
}
