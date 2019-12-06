using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.CodeGeneration.CodeGenerators
{
    public class SQLClassGenerator
    {
        SqlTypeConverter typeConverter = new SqlTypeConverter();

        public Dictionary<string, string> GenerateMultiFile(string strConn)
        {

            var result = new Dictionary<string, string>();
            var con = new SqlConnection(strConn);
            con.Open();
            string[] objArrRestrict;
            DataTable schemaTbl;
            schemaTbl = con.GetSchema("Tables", null);
            //var rows = schemaTbl.Rows.OfType<DataRow>().OrderBy(a => a["TABLE_NAME"]);
            foreach (DataRow row in schemaTbl.Rows)
            {
                StringBuilder sb = new StringBuilder();
                bool hasname = false;
                var tablename = (string)row["TABLE_NAME"];
                var skipColumns = new []{ "id", "created", "changed", "createdby", "changedby", "Intid" };

                objArrRestrict = new string[] { null, null, tablename, null };
                var tbl = con.GetSchema("Columns", objArrRestrict);
                foreach (DataRow colrow in tbl.Rows)
                    hasname |= (string)colrow["column_name"] == "adi";
                sb.AppendFormat("    public partial class {0} : InfolineTable", tablename);sb.AppendLine();
                sb.AppendLine  ("    {");
                foreach (DataRow colrow in tbl.Rows)
                {
                    var cname = (string)colrow["column_name"];
                    if (skipColumns.Contains(cname)) continue;
                    if ((string)colrow["data_type"] != "geography")
                    {
                        Type stype = typeConverter.Convert((string)colrow["data_type"]);
                        string alias = typeConverter.GetAlias(stype);
                        alias += ((string)colrow["is_nullable"]) == "YES" && !stype.IsClass ? "?" : "";
                        sb.AppendFormat("        public {0} {1} {{ get; set;}}", alias, cname);
                        sb.AppendLine();
                    }
                    else
                    {
                        sb.AppendFormat("        public {0} {1} {{ get; set;}}", "SqlGeography ", cname);
                        sb.AppendLine();

                    }

                }
                sb.AppendLine("    }");
                result.Add(tablename, sb.ToString());
            }
            result.Add("InfolineTable", GetBaseTable());
            return result;
        }
        public string GetObject(string strConn, string tablename)
        {
            string result = "";
            var con = new SqlConnection(strConn);
            con.Open();
            string[] objArrRestrict;
            DataTable schemaTbl;
            schemaTbl = con.GetSchema("Tables", null);
            foreach (DataRow row in schemaTbl.Rows)
            {
                if ((string)row["TABLE_NAME"] == tablename)
                {
                    StringBuilder sb = new StringBuilder();
                    bool hasname = false;
                   
                    objArrRestrict = new string[] { null, null, tablename, null };
                    var tbl = con.GetSchema("Columns", objArrRestrict);
                    foreach (DataRow colrow in tbl.Rows)
                        hasname |= (string)colrow["column_name"] == "adi";
                    sb.AppendFormat("    public class {0}", tablename); sb.AppendLine();
                    sb.AppendLine("    {");
                    foreach (DataRow colrow in tbl.Rows)
                    {
                        var cname = (string)colrow["column_name"];
                        if ((string)colrow["data_type"] != "geography")
                        {
                            Type stype = typeConverter.Convert((string)colrow["data_type"]);
                            string alias = typeConverter.GetAlias(stype);
                            alias += ((string)colrow["is_nullable"]) == "YES" && !stype.IsClass ? "?" : "";
                            sb.AppendFormat("        public {0} {1} {{ get; set;}}", alias, cname);
                            sb.AppendLine();
                        }
                        else
                        {
                            sb.AppendFormat("        public {0} {1} {{ get; set;}}", "SqlGeography ", cname);
                            sb.AppendLine();
                        }
                    }
                    sb.AppendLine("    }");
                    result= sb.ToString();
                }
            }
            return result;
        }
        public string GetBaseTable()
        {
            var str = @"
    
	public class InfolineTable
    {
        public InfolineTable()
        {
            id = Guid.NewGuid();
        }
        public Guid id { get; set; }
        public DateTime? created { get; set; }
        public DateTime? changed { get; set; }
        public Guid? changedby { get; set; }
        public Guid? createdby { get; set; }
    }
";
            return str;
        }
        
        public string Generate(string strConn)
        {
            var classes = GenerateMultiFile(strConn);
            return string.Join("\r\n", classes.OrderBy(a => a.Key).Select(a => a.Value));
        }



    }
}
