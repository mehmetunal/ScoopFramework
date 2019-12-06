using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Infoline.Framework.CodeGeneration.CodeGenerators
{
    public class SQLServiceGenerator
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
            var classname = con.Database;
            foreach (DataRow row in schemaTbl.Rows)
            {
                StringBuilder sb = new StringBuilder();
                bool hasname = false;
                var tablename = (string)row["TABLE_NAME"];
                var tabletype = (string)row["TABLE_TYPE"]; // "BASE TABLE", "VIEW"

                if ((string)row["TABLE_NAME"] != "sysdiagrams")
                {
                    objArrRestrict = new string[] { null, null, tablename, null };
                    var tbl = con.GetSchema("Columns", objArrRestrict);
                    foreach (DataRow colrow in tbl.Rows)
                        hasname |= (string)colrow["column_name"] == "adi";

                    sb.AppendFormat("    [Export(typeof(ISmartHandler))]"); sb.AppendLine();
                    sb.AppendFormat("    public partial class {0}Handler : BaseSmartHandler", tablename); sb.AppendLine();
                    sb.AppendLine  ("    {");
                    sb.AppendFormat("        public {0}Handler()", tablename); sb.AppendLine();
                    sb.AppendFormat("            : base(\"{0}\")", tablename); sb.AppendLine();
                    sb.AppendLine  ("        {");
                    sb.AppendLine  ();
                    sb.AppendLine  ("        }");
                    sb.AppendLine  ();
                    
                    sb.AppendFormat ("        [HandleFunction(\"{0}/GetAll\")]", tablename); sb.AppendLine();
                    sb.AppendFormat ("        public void {0}GetAll(HttpContext context)", tablename); sb.AppendLine();
                    sb.AppendLine   ("        {");

                    sb.AppendLine   ("            var cond = ParseRequest<Condition>(context);");
                    sb.AppendFormat ("            var db = new {0}Database();", classname); sb.AppendLine();
                    sb.AppendFormat ("            var data = db.Get{0}(cond);", tablename); sb.AppendLine();
                    sb.AppendLine   ("            RenderResponse(context, data);");
                    sb.AppendLine   ("        }"); sb.AppendLine();

                    if (tabletype == "BASE TABLE")
                    {

                        sb.AppendFormat("        [HandleFunction(\"{0}/GetById\")]", tablename); sb.AppendLine();
                        sb.AppendFormat("        public void {0}GetById(HttpContext context)", tablename); sb.AppendLine();
                        sb.AppendLine("        {");
                        sb.AppendFormat("            var db = new {0}Database();", classname); sb.AppendLine();
                        sb.AppendFormat("            var id = context.Request[\"id\"];"); sb.AppendLine();
                        sb.AppendFormat("            var data = db.Get{0}ById(new Guid((string)id));", tablename); sb.AppendLine();

                        sb.AppendLine("            RenderResponse(context, data);"); sb.AppendLine();
                        sb.AppendLine("        }");
                        sb.AppendLine();

                        sb.AppendFormat("        [HandleFunction(\"{0}/Insert\")]", tablename); sb.AppendLine();
                        sb.AppendFormat("        public void {0}Insert(HttpContext context)", tablename); sb.AppendLine();
                        sb.AppendLine("        {");
                        sb.AppendFormat("            var db = new {0}Database();", classname); sb.AppendLine();
                        sb.AppendFormat("            var data = ParseRequest<{0}>(context);", tablename); sb.AppendLine();

                        sb.AppendFormat("            RenderResponse(context, db.Insert{0}(data));", tablename); sb.AppendLine();
                        sb.AppendLine("        }");
                        sb.AppendLine();

                        sb.AppendFormat("        [HandleFunction(\"{0}/Update\")]", tablename); sb.AppendLine();
                        sb.AppendFormat("        public void {0}Update(HttpContext context)", tablename); sb.AppendLine();
                        sb.AppendLine("        {");
                        sb.AppendFormat("            var db = new {0}Database();", classname); sb.AppendLine();
                        sb.AppendFormat("            var data = ParseRequest<{0}>(context);", tablename); sb.AppendLine();
                        sb.AppendFormat("            RenderResponse(context, db.Update{0}(data));", tablename); sb.AppendLine();
                        sb.AppendLine("        }");
                        sb.AppendLine();

                        sb.AppendFormat("        [HandleFunction(\"{0}/Delete\")]", tablename); sb.AppendLine();
                        sb.AppendFormat("        public void {0}Delete(HttpContext context)", tablename); sb.AppendLine();
                        sb.AppendLine("        {");
                        sb.AppendFormat("            var db = new {0}Database();", classname); sb.AppendLine();
                        sb.AppendFormat("            var id = context.Request[\"id\"];"); sb.AppendLine();
                        sb.AppendLine("            if(id != null)");
                        sb.AppendLine("            {");
                        sb.AppendFormat("                RenderResponse(context, db.Delete{0}(new Guid((string)id)));", tablename); sb.AppendLine();
                        sb.AppendLine("            }");
                        sb.AppendLine("            else");
                        sb.AppendLine("            {");
                        sb.AppendFormat("                var item = ParseRequest<{0}>(context);", tablename); sb.AppendLine();
                        sb.AppendFormat("                RenderResponse(context, db.Delete{0}(item));", tablename); sb.AppendLine();
                        sb.AppendLine("            }");
                        sb.AppendLine("        }");
                        sb.AppendLine();
                    }
                    sb.AppendLine("    }");
                    result.Add(tablename, sb.ToString());
                }
            }
            return result;
        }

    }
}
