using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.CodeGeneration.CodeGenerators
{
    public class SQLDatabaseFunctions
    {
        public Dictionary<string, string> GenerateMultiFile(string strConn)
        {
            var result = new Dictionary<string, string>();
            var con = new SqlConnection(strConn);
            con.Open();
            string[] objArrRestrict;
            DataTable schemaTbl;
            schemaTbl = con.GetSchema("Tables", null);
            var classname = con.Database;
            foreach (DataRow row in schemaTbl.Rows)
            {
                StringBuilder sb = new StringBuilder();
                bool hasname = false;
                var tablename = (string)row["TABLE_NAME"];
                var tabletype = (string)row["TABLE_TYPE"]; // "BASE TABLE", "VIEW"
                objArrRestrict = new string[] { null, null, tablename, null };
                var tbl = con.GetSchema("Columns", objArrRestrict);
                foreach (DataRow colrow in tbl.Rows)
                    hasname |= (string)colrow["column_name"] == "adi";
                

                sb.AppendFormat("    partial class {0}Database", classname);
                sb.AppendLine();
                sb.AppendLine("    {");
                sb.AppendFormat("        public {0}[] Get{0}()", tablename); sb.AppendLine();
                sb.AppendLine("        {");
                sb.AppendLine("            using (var db = GetDB())");
                sb.AppendLine("            {");
                sb.AppendFormat("                return db.Select<{0}>().OrderDescBy(a => a.created).Execute().ToArray();", tablename); sb.AppendLine();
                sb.AppendLine("            }");
                sb.AppendLine("        }");
                sb.AppendLine();

                sb.AppendFormat("        public Dictionary<string, object>[] Get{0}(Condition condition)", tablename); sb.AppendLine();
                sb.AppendLine("        {");
                sb.AppendLine("            using (var db = GetDB())");
                sb.AppendLine("            {"); sb.AppendLine();
                sb.AppendLine("                condition = condition ?? new Condition();");
                sb.AppendFormat("                IExecuteReader<Dictionary<string, object>> query = db.Select<Dictionary<string, object>>(\"{0}\", condition.Fields);", tablename); sb.AppendLine();

                sb.AppendLine("                if (condition != null && condition.Filter != null && condition.Filter.Count() > 0)");
                sb.AppendLine("                {");
                sb.AppendLine("                    foreach (var item in condition.Filter)");
                sb.AppendLine("                    {");
                sb.AppendLine("                        var cond = new ColumnCompareCondition(item.Field, item.Value, item.Operator);");
                sb.AppendLine("                        query = query.Where(cond);");
                sb.AppendLine("                    }");
                sb.AppendLine("                }");

                sb.AppendLine("                if (condition.Sort != null)");
                sb.AppendLine("                    query = query.OrderBy(condition.Sort.Field, condition.Sort.Type);");

                sb.AppendLine("                query = query.Skip(condition.StartIndex);");
                sb.AppendLine("                query = query.Take(condition.Count);");
                sb.AppendLine("                var result = query.Execute().ToArray();");
                sb.AppendLine("                return result;");
                sb.AppendLine("            }");
                sb.AppendLine("        }");

                if (tabletype == "BASE TABLE")
                {
                    sb.AppendFormat("        public {0} Get{0}ById(Guid id)", tablename); sb.AppendLine();
                    sb.AppendLine("        {");
                    sb.AppendLine("            using (var db = GetDB())"); sb.AppendLine();
                    sb.AppendLine("            {");
                    sb.AppendFormat("                return db.Select<{0}>().Where(a => a.id == id).Execute().FirstOrDefault();", tablename); sb.AppendLine();
                    sb.AppendLine("            }");
                    sb.AppendLine("        }");
                    sb.AppendLine();

                    sb.AppendFormat("        public ResultStatus Insert{0}({0} item)", tablename); sb.AppendLine();
                    sb.AppendLine("        {");
                    sb.AppendLine("            using (var db = GetDB())");
                    sb.AppendLine("            {");
                    sb.AppendFormat("                return db.ExecuteInsert<{0}>(item);", tablename); sb.AppendLine();
                    sb.AppendLine("            }");
                    sb.AppendLine("        }");
                    sb.AppendLine();

                    sb.AppendFormat("        public ResultStatus Update{0}({0} item, bool setNull = false)", tablename); sb.AppendLine();
                    sb.AppendLine("        {");
                    sb.AppendLine("            using (var db = GetDB())");
                    sb.AppendLine("            {");
                    sb.AppendFormat("                return db.ExecuteUpdate<{0}>(item, setNull);", tablename); sb.AppendLine();
                    sb.AppendLine("            }");
                    sb.AppendLine("        }");
                    sb.AppendLine();

                    sb.AppendFormat("        public ResultStatus Delete{0}(Guid id)", tablename); sb.AppendLine();
                    sb.AppendLine("        {");
                    sb.AppendLine("            using (var db = GetDB())");
                    sb.AppendLine("            {");
                    sb.AppendFormat("                return db.ExecuteDelete<{0}>(id);", tablename); sb.AppendLine();
                    sb.AppendLine("            }");
                    sb.AppendLine("        }");
                    sb.AppendLine();

                    sb.AppendFormat("        public ResultStatus Delete{0}({0} item)", tablename); sb.AppendLine();
                    sb.AppendLine("        {");
                    sb.AppendLine("            using (var db = GetDB())");
                    sb.AppendLine("            {");
                    sb.AppendFormat("                return db.ExecuteDelete<{0}>(item);", tablename); sb.AppendLine();
                    sb.AppendLine("            }");
                    sb.AppendLine("        }");
                    sb.AppendLine();

                    sb.AppendFormat("        public ResultStatus BulkInsert{0}(IEnumerable<{0}> item)", tablename); sb.AppendLine();
                    sb.AppendLine("        {");
                    sb.AppendLine("            using (var db = GetDB())");
                    sb.AppendLine("            {");
                    sb.AppendFormat("                return db.ExecuteBulkInsert<{0}>(item);", tablename); sb.AppendLine();
                    sb.AppendLine("            }");
                    sb.AppendLine("        }");
                    sb.AppendLine();

                    sb.AppendFormat("        public ResultStatus BulkUpdate{0}(IEnumerable<{0}> item, bool setNull = false)", tablename); sb.AppendLine();
                    sb.AppendLine("        {");
                    sb.AppendLine("            using (var db = GetDB())");
                    sb.AppendLine("            {");
                    sb.AppendFormat("                return db.ExecuteBulkUpdate<{0}>(item, setNull);", tablename); sb.AppendLine();
                    sb.AppendLine("            }");
                    sb.AppendLine("        }");
                    sb.AppendLine();

                    sb.AppendFormat("        public ResultStatus BulkDelete{0}(IEnumerable<{0}> item)", tablename); sb.AppendLine();
                    sb.AppendLine("        {");
                    sb.AppendLine("            using (var db = GetDB())");
                    sb.AppendLine("            {");
                    sb.AppendFormat("                return db.ExecuteBulkDelete<{0}>(item);", tablename); sb.AppendLine();
                    sb.AppendLine("            }");
                    sb.AppendLine("        }");
                    sb.AppendLine();
                }
                sb.AppendLine("    }");

                result.Add(tablename, sb.ToString());
            }
            result.Add(classname, GetDefaultClass(classname, strConn));
            result.Add("InfolineDatabase", GetDatabaseWrapper());
            return result;
        }

        public string GetDefaultClass(string dbname, string strConn)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("   public partial class {0}Database", dbname); sb.AppendLine();
            sb.AppendLine("   {");
            sb.AppendLine("       public string ConnectionString { get; private set; }");
            sb.AppendLine("       ");
            sb.AppendFormat("       public {0}Database()", dbname);
            sb.AppendLine();
            sb.AppendLine("       {");


            sb.AppendLine("if (System.Configuration.ConfigurationManager.ConnectionStrings[\"DatabaseConnection\"] != null)");
            sb.AppendLine("    this.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[\"DatabaseConnection\"].ConnectionString;");
            sb.AppendLine();

            sb.AppendLine("#if DEBUG");
            sb.AppendFormat("           ConnectionString =  \"{0}\";", strConn);
            sb.AppendLine();
            sb.AppendLine("#endif");

            sb.AppendLine();
            sb.AppendLine("       }");
            sb.AppendLine("       ");
            sb.AppendFormat("       public {0}Database(string conn)", dbname); sb.AppendLine();
            sb.AppendLine("       {");
            sb.AppendLine("           this.ConnectionString = conn;");
            sb.AppendLine("       }");
            sb.AppendLine();
            sb.AppendLine("       public InfolineDatabase GetDB()");
            sb.AppendLine("       {");
            sb.AppendLine("           return new InfolineDatabase(ConnectionString);");
            sb.AppendLine("       }");
            sb.AppendLine("   }");

            return sb.ToString();

        }

        public string GetDatabaseWrapper()
        {
            var str = @"
	public class InfolineDatabase : IDisposable
    {
        string _connectionString;
        Database _database;

        public InfolineDatabase(string connectionString)
        {
            _connectionString = connectionString;
            _database = new Database(connectionString);
        }

        public void Dispose()
        {
            _database.Dispose();
        }

        public int ExecuteNonQuery(string txt, params object[] parameters)
        {
            return _database.ExecuteNonQuery(txt, parameters);
        }

        public T ExecuteScaler<T>(string txt, params object[] parameters)
        {
            return _database.ExecuteScaler<T>(txt, parameters);
        }

        public IEnumerable<Dictionary<string, object>> ExecuteReader(string txt, params object[] parameters)
        {
            return _database.ExecuteReader(txt, parameters);
        }

        public IEnumerable<T> ExecuteReader<T>(string txt, params object[] parameters) where T : new()
        {
            return _database.ExecuteReader<T>(txt, parameters);
        }
        
        public ResultStatus ExecuteInsert<T>(T parameter, string tableName = null) where T : InfolineTable
        {
            return _database.ExecuteInsert(parameter,null,  tableName);
        }

        public ResultStatus ExecuteUpdate<T>(T parameter, bool setNull = false, string tableName = null) where T : InfolineTable
        {
            return _database.ExecuteUpdate(parameter, a => a.id, a => new { a.created, a.id }, setNull, tableName);
        }

        public ResultStatus ExecuteDelete<T>(T parameter, string tableName = null) where T : InfolineTable
        {
            return _database.ExecuteDelete(parameter, a => a.id, tableName);
        }

        public ResultStatus ExecuteDelete<T>(Guid id, string tableName = null) where T : InfolineTable, new()
        {
            return _database.ExecuteDelete(new T { id = id }, a => a.id, tableName);
        }

        public ResultStatus ExecuteBulkInsert<T>(IEnumerable<T> parameter, string tableName = null) where T : InfolineTable
        {
            return _database.ExecuteBulkInsert<T>(parameter.ToList(), null, tableName);
        }

        public ResultStatus ExecuteBulkUpdate<T>(IEnumerable<T> parametre, bool setNull = false, string tableName = null) where T : InfolineTable
        {
            return _database.ExecuteBulkUpdate<T>(parametre.ToList(), a => a.id, a => new { a.created, a.id }, setNull, tableName);
        }

        public ResultStatus ExecuteBulkDelete<T>(IEnumerable<T> parametre, string tableName = null) where T : InfolineTable
        {
            return _database.ExecuteBulkDelete<T>(parametre.ToList(), a => a.id, tableName);
        }

        public ISelect<T> Select<T>(System.Linq.Expressions.Expression<Func<T, object>> columns = null, string tablename = null) where T : new()
        {
            return _database.Select<T>(columns, tablename);
        }

        public ISelect<T> Select<T>(string name, string[] columns) where T : new()
        {
            return _database.Select<T>(columns, name);
        }

        public ResultStatus CreateTable<T>() where T : InfolineTable
        {
            return _database.CreateTable<T>()
                            .SetPrimaryKey(a => a.id)
                            .SetDefaultValue(a => a.id, SqlServerFunctions.NEWID)
                            .SetDefaultValue(a => a.created, SqlServerFunctions.GETDATE)
                            .SetDefaultValue(a => a.createdby, SqlServerFunctions.GETDATE)
                            .Execute();
        }
    }
";
            return str;
        }

    }
}
