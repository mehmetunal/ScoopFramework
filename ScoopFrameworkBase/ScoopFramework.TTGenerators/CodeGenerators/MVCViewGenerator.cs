using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.CodeGeneration.CodeGenerators
{
    public class MVCViewGenerator
    {
        public Dictionary<string, string> GenerateMultiFile(string strConn, string bussinessNameSpace, Dictionary<string, string> tables)
        {
            var result = new Dictionary<string, string>();
            var con = new SqlConnection(strConn);
            con.Open();
            string[] objArrRestrict;
            var dbname = con.Database;

            var schemaTbl = con.GetSchema("Tables", null);
            var rows = schemaTbl.Rows.Cast<DataRow>().Where(a => tables.Keys.Contains(a["TABLE_NAME"]));
            foreach (DataRow row in rows)
            {
                
                var tablename = (string)row["TABLE_NAME"];
                var classname = tables[tablename];

                objArrRestrict = new string[] { null, null, tablename, null };
                var tbl = con.GetSchema("Columns", objArrRestrict);

                var index = GetIndex(bussinessNameSpace, tablename, classname, tbl);
                var insert = GetInsert(bussinessNameSpace, tablename, classname, tbl);
                var update = GetUpdate(bussinessNameSpace, tablename, classname, tbl);

                result.Add(string.Format(@"{0}\Index.cshtml", classname), index);
                result.Add(string.Format(@"{0}\Insert.cshtml", classname), insert);
                result.Add(string.Format(@"{0}\Update.cshtml", classname), update);
            }
            return result;
        }

        public string GetIndex(string bussinessNameSpace, string tablename, string classname, DataTable columns)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("@model IEnumerable<{0}.{1}>", bussinessNameSpace, tablename); sb.AppendLine();
            sb.AppendLine  ("@{");
            sb.AppendLine  ("    ViewBag.Title = \"Index\";");
            sb.AppendLine  ("    Layout = \"~/Views/Shared/MasterPage.cshtml\";");
            sb.AppendLine  ("}");
            sb.AppendLine  ();
            
            sb.AppendLine  ("<div class=\"container\">");
            sb.AppendLine  ("   <div class=\"row\">");
            sb.AppendLine  ("       <div class=\"col-md-12\">");
            sb.AppendLine  ();

            sb.AppendLine  ("@(Html.Kendo()");
            sb.AppendLine  ("      .Grid(Model)");
            sb.AppendFormat("      .Name(\"{0}\")", classname); sb.AppendLine();
            sb.AppendFormat("      .DataSource(item => item.Ajax().Read(r => r.Action(\"GridSource\", \"{0}\")).PageSize(10))", classname); sb.AppendLine();
            sb.AppendLine  ("      .Columns(item =>");
            sb.AppendLine  ("      {");
            var cols1 = columns.Rows.OfType<DataRow>().Where(a => ((string)a["column_name"]) == "id");
            var cols2 = columns.Rows.OfType<DataRow>().Where(a => ((string)a["column_name"]) != "id");
            var cols = cols1.Union(cols2);
            foreach (DataRow colrow in cols)
            {
                var cname = (string)colrow["column_name"];
                var dtype = (string)colrow["data_type"];
                sb.AppendFormat("        item.Bound(y => y.{0})", cname);
                if (cname != "id")
                {
                    sb.AppendFormat(".Title(\"{0}\")", cname);
                    if (dtype == "datetime")
                        sb.Append(".Format(\"{0:dd.MM.yyyy - HH:mm}\")");
                }                    
                else
                {
                    sb.Append(".Title(\"\")");
                    sb.Append(".ClientTemplate(\"<input type=\\\"checkbox\\\" data-event=\\\"GridSelector\\\" />\")");
                    sb.Append(".Width(40)");
                    sb.Append(".Sortable(false)");
                    sb.Append(".Filterable(false)");
                    sb.Append(".HtmlAttributes(new Dictionary<string, object>() { { \"class\", \"text-center\" } })");
                }
                sb.Append(";");
                sb.AppendLine();
            }
            sb.AppendLine  ("      })");
            sb.AppendLine  ("      .Selectable(x => x.Mode(GridSelectionMode.Multiple))");
            sb.AppendLine  ("      .Sortable()");
            sb.AppendLine  ("      .Filterable()");
            sb.AppendLine  ("      .Navigatable()");
            sb.AppendLine  ("      .Pageable(x => x.PageSizes(true))");
            sb.AppendLine  ("      .ToolBar(item =>");
            sb.AppendLine  ("      {");
            sb.AppendLine  ("          item.Custom().Text(\"<i class=\\\"fa fa-insert\\\"></i>Ekle\")");
            sb.AppendLine  ("              .HtmlAttributes(new Dictionary<string, object>() { ");
            sb.AppendLine  ("                                     { \"data-task\", \"Insert\" }, ");
            sb.AppendLine  ("                                     { \"data-opentype\", \"modal\" }, ");
            sb.AppendFormat("                                     {{ \"data-title\", \"Yeni {0} Ekle\" }}, ", tablename); sb.AppendLine();
            sb.AppendFormat("                                     {{ \"data-target\", Url.Action(\"Insert\", \"{0}\") }} }}).Url(\"#\");", classname); sb.AppendLine();
            sb.AppendLine  ();
            sb.AppendLine  ("          item.Custom().Text(\"Düzenle\")");
            sb.AppendLine  ("              .HtmlAttributes(new Dictionary<string, object>() { ");
            sb.AppendLine  ("                                     { \"data-task\", \"Update\" }, ");
            sb.AppendLine  ("                                     { \"class\", \"hide\" }, ");
            sb.AppendLine  ("                                     { \"data-opentype\", \"modal\" }, ");
            sb.AppendFormat("                                     {{ \"data-title\", \"{0} Düzenle\" }}, ", tablename); sb.AppendLine();
            sb.AppendFormat("                                     {{ \"data-target\", Url.Action(\"Update\", \"{0}\") }} }}).Url(\"#\");", classname); sb.AppendLine();
            sb.AppendLine  ();
            sb.AppendLine  ("          item.Custom().Text(\"Sil\")");
            sb.AppendLine  ("              .HtmlAttributes(new Dictionary<string, object>() { ");
            sb.AppendLine  ("                                     { \"data-task\", \"Delete\" }, ");
            sb.AppendLine  ("                                     { \"data-id\", \"000\" }, ");
            sb.AppendFormat("                                     {{ \"data-target\", Url.Action(\"Delete\", \"{0}\") }} }}).Url(\"#\");", classname); sb.AppendLine();
            sb.AppendLine  ();
            sb.AppendLine  ("      })");
            sb.AppendLine  ("      .ToolBar(x =>");
            sb.AppendLine  ("      {");
            sb.AppendLine  ("          x.Pdf().Text(\"PDF'e Aktar\");");
            sb.AppendLine  ("          x.Excel().Text(\"Excel'e Aktar\");");
            sb.AppendLine  ("      })");
            sb.AppendLine  ("      .Events(x =>");
            sb.AppendLine  ("      {");
            sb.AppendLine  ("          x.DataBound(\"LoadGridData\");");
            sb.AppendLine  ("          x.Change(\"GridChange\");");
            sb.AppendLine  ("      })");
            sb.AppendLine  (")");
            sb.AppendLine  ("        </div>");
            sb.AppendLine  ("    </div>");
            sb.AppendLine  ("</div>");


            return sb.ToString();
        }

        public string GetInsert(string bussinessNameSpace, string tablename, string classname, DataTable columns)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("@model {0}.{1}", bussinessNameSpace, tablename); sb.AppendLine();
            sb.AppendLine  ("@{");
            sb.AppendLine  ("    ViewBag.Title = \"Insert\";");
            sb.AppendLine  ("    Layout = \"~/Views/Shared/MasterPage.cshtml\";");
            sb.AppendLine  ("}");
            sb.AppendLine  ();
            
            sb.AppendLine  ("<div class=\"container\">");

            sb.AppendFormat("@using (Html.BeginForm(\"Insert\", \"{0}\", FormMethod.Post, new Dictionary<string, object>() {{", classname); sb.AppendLine();
            sb.AppendLine  ("    {\"role\",         \"form\"},");
            sb.AppendLine  ("    {\"data-event\",   \"ValidForm\"},");
            sb.AppendLine  ("    {\"class\",        \"form-horizontal\"},");
            sb.AppendLine  ("    {\"data-selector\", \"modalContainer\"}");
            sb.AppendLine  ("}))");
            sb.AppendLine  ("{");
            sb.AppendLine  ("   @Html.AntiForgeryToken()");
            sb.AppendLine  ("   <div class=\"clearfix\">");

            var cols1 = columns.Rows.OfType<DataRow>().Where(a => ((string)a["column_name"]) == "id");
            var cols2 = columns.Rows.OfType<DataRow>().Where(a => ((string)a["column_name"]) != "id");
            var cols = cols1.Union(cols2);
            foreach (DataRow colrow in cols)
            {
                var cname = (string)colrow["column_name"];
                var dtype = (string)colrow["data_type"];
                if (cname != "id")
                {
                    sb.AppendLine  ("        <div class=\"form-group\">");
                    sb.AppendLine  ("            <div class=\"col-md-4\">");
                    sb.AppendFormat("                <label class=\"control-label label-md\" for=\"{0}\">{0}</label>", cname); sb.AppendLine();
                    sb.AppendLine  ("            </div>");
                    sb.AppendLine  ("            <div class=\"col-md-8\">");
                    sb.AppendFormat("                @Html.TextBoxFor(a => a.{0}, new Dictionary<string, object>()", cname); sb.AppendLine();
                    sb.AppendLine  ("                {");
                    sb.AppendFormat("                    {{ \"placeholder\", \"{0}\" }},", cname); sb.AppendLine();
                    sb.AppendLine  ("                    { \"class\", \"form-control\" },");
                    sb.AppendLine  ("                    { \"required\", \"required\" }, // Zorunlululuk default");
                    sb.AppendLine  ("                    //{ \"maxlength\", \"0\" },     // Minimum string uzunluk");
                    sb.AppendLine  ("                    //{ \"minlength\", \"11\" },    // Maximum string uzunluk");
                    sb.AppendLine  ("                    //{ \"min\", \"0\" },           // Minimum int değer");
                    sb.AppendLine  ("                    //{ \"max\", \"11\" }           // Masimum int değer");
                    sb.AppendLine  ("                })");
                    sb.AppendLine  ("            </div>");
                    sb.AppendLine  ("        </div>");
                }                    
                else
                {
                    continue;
                }
                sb.AppendLine();
            }
            sb.AppendLine  ("   </div>");
            sb.AppendLine  ("   <div class=\"clearfix p-t-lg\">");
            sb.AppendLine  ("       <div class=\"row\">");
            sb.AppendLine  ("           <div class=\"col-md-2 pull-left\">");
            sb.AppendLine  ("               <button type=\"button\" class=\"btn btn-md btn-block btn-default\" data-dismiss=\"modal\"><i class=\"fa fa-chevron-left\"></i> Geri</button>");
            sb.AppendLine  ("           </div>");
            sb.AppendLine  ("           <div class=\"col-md-2 pull-right\">");
            sb.AppendLine  ("               <button type=\"submit\" class=\"btn btn-md btn-block btn-default\"><i class=\"fa fa-save\"></i> Kaydet</button>");
            sb.AppendLine  ("           </div>");
            sb.AppendLine  ("       </div>");
            sb.AppendLine  ("   </div>");
            sb.AppendLine  ("}");
            sb.AppendLine  ("</div>");

            return sb.ToString();
        }

        public string GetUpdate(string bussinessNameSpace, string tablename, string classname, DataTable columns)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("@model {0}.{1}", bussinessNameSpace, tablename); sb.AppendLine();
            sb.AppendLine  ("@{");
            sb.AppendLine  ("    ViewBag.Title = \"Update\";");
            sb.AppendLine  ("    Layout = \"~/Views/Shared/MasterPage.cshtml\";");
            sb.AppendLine  ("}");
            sb.AppendLine  ();
            
            sb.AppendLine  ("<div class=\"container\">");

            sb.AppendFormat("@using (Html.BeginForm(\"Update\", \"{0}\", FormMethod.Post, new Dictionary<string, object>() {{", classname); sb.AppendLine();
            sb.AppendLine  ("    {\"role\",         \"form\"},");
            sb.AppendLine  ("    {\"data-event\",   \"ValidForm\"},");
            sb.AppendLine  ("    {\"class\",        \"form-horizontal\"},");
            sb.AppendLine  ("    {\"data-selector\", \"modalContainer\"}");
            sb.AppendLine  ("}))");
            sb.AppendLine  ("{");
            sb.AppendLine  ("   @Html.AntiForgeryToken()");
            sb.AppendLine  ("   @Html.HiddenFor(a => a.id)");
            sb.AppendLine  ("   <div class=\"clearfix\">");

            var cols1 = columns.Rows.OfType<DataRow>().Where(a => ((string)a["column_name"]) == "id");
            var cols2 = columns.Rows.OfType<DataRow>().Where(a => ((string)a["column_name"]) != "id");
            var cols = cols1.Union(cols2);
            foreach (DataRow colrow in cols)
            {
                var cname = (string)colrow["column_name"];
                var dtype = (string)colrow["data_type"];
                if (cname != "id")
                {
                    sb.AppendLine  ("        <div class=\"form-group\">");
                    sb.AppendLine  ("            <div class=\"col-md-4\">");
                    sb.AppendFormat("                <label class=\"control-label label-md\" for=\"{0}\">{0}</label>", cname); sb.AppendLine();
                    sb.AppendLine  ("            </div>");
                    sb.AppendLine  ("            <div class=\"col-md-8\">");
                    sb.AppendFormat("                @Html.TextBoxFor(a => a.{0}, new Dictionary<string, object>()", cname); sb.AppendLine();
                    sb.AppendLine  ("                {");
                    sb.AppendFormat("                    {{ \"placeholder\", \"{0}\" }},", cname); sb.AppendLine();
                    sb.AppendLine  ("                    { \"class\", \"form-control\" },");
                    sb.AppendLine  ("                    { \"required\", \"required\" }, // Zorunlululuk default");
                    sb.AppendLine  ("                    //{ \"maxlength\", \"0\" },     // Minimum string uzunluk");
                    sb.AppendLine  ("                    //{ \"minlength\", \"11\" },    // Maximum string uzunluk");
                    sb.AppendLine  ("                    //{ \"min\", \"0\" },           // Minimum int değer");
                    sb.AppendLine  ("                    //{ \"max\", \"11\" }           // Masimum int değer");
                    sb.AppendLine  ("                })");
                    sb.AppendLine  ("            </div>");
                    sb.AppendLine  ("        </div>");
                }                    
                else
                {
                    continue;
                }
                sb.AppendLine();
            }
            sb.AppendLine  ("   </div>");
            sb.AppendLine  ("   <div class=\"clearfix p-t-lg\">");
            sb.AppendLine  ("       <div class=\"row\">");
            sb.AppendLine  ("           <div class=\"col-md-2 pull-left\">");
            sb.AppendLine  ("               <button type=\"button\" class=\"btn btn-md btn-block btn-default\" data-dismiss=\"modal\"><i class=\"fa fa-chevron-left\"></i> Geri</button>");
            sb.AppendLine  ("           </div>");
            sb.AppendLine  ("           <div class=\"col-md-2 pull-right\">");
            sb.AppendLine  ("               <button type=\"submit\" class=\"btn btn-md btn-block btn-default\"><i class=\"fa fa-save\"></i> Kaydet</button>");
            sb.AppendLine  ("           </div>");
            sb.AppendLine  ("       </div>");
            sb.AppendLine  ("   </div>");
            sb.AppendLine  ("}");
            sb.AppendLine  ("</div>");

            return sb.ToString();
        }
    }
}
