using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.CodeGeneration.CodeGenerators
{
    public class MVCPageSetup
    {
        public string Area { get; set; }
        public string ViewName { get; set; }
        public string TableName { get; set; }
        public bool List { get; set; }
        public bool Detail { get; set; }
        public bool Create { get; set; }
        public bool Delete { get; set; }
        public bool Update { get; set; }

        public MVCPageSetup()
        {

        }

        public MVCPageSetup(string area, string view, string table, bool list, bool detail, bool create, bool update, bool delete)
        {
            Area = area;
            ViewName = view;
            TableName = table;
            List = list;
            Detail = detail;
            Create = create;
            Update = update;
            Delete = delete;
        }
    }

    public class MVCGenerator
    {

        public Dictionary<string, string> GenerateControllers(string strConn, string solutionName, string projectName, object[] obj_tables)
        {
            var result = new Dictionary<string, string>();
            if (obj_tables == null) return result;
            var tables = obj_tables.Cast<MVCPageSetup>().ToArray();

            var con = new SqlConnection(strConn); con.Open();
            var dbname = con.Database;
            //var schemaTbl = con.GetSchema("Tables", new string[] { null, null, null, "VIEW" });
            //var rows = schemaTbl.Rows.Cast<DataRow>().Where(a => tables.Select(b => b.ViewName).Contains(a["TABLE_NAME"]));

            for (int i = 0; i < tables.Length; i++)
            {
                var table = tables[i];
                var tableName = table.TableName;
                var viewName = table.ViewName;
                var areaName = table.Area;
                var controllerLocation = string.Format("Areas/{0}/Controllers/{1}Controller.cs", areaName, viewName);

                var controllerContent = GetController(dbname, solutionName, projectName, table);
                result.Add(controllerLocation, controllerContent);
            }

            var areas = tables.Select(a => a.Area).Distinct();
            foreach (var area in areas)
            {
                var registrationLoacation = string.Format("Areas/{0}/{0}AreaRegistration.cs", area);
                var webConfigLocation = string.Format("Areas/{0}/Views/Web.config", area);
                var registrationContent = GetAreaRegistration(projectName, area);
                var webConfigContent = GetWebConfig();
                result.Add(registrationLoacation, registrationContent);
                result.Add(webConfigLocation, webConfigContent);
            }

            return result;
        }


        private string GetController(string dbname, string solutionName, string projectName, MVCPageSetup table)
        {
            var tableName = table.TableName;
            var viewName = table.ViewName;
            var areaName = table.Area;
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(GetMethod_Constructer());
            sb.AppendLine();
            sb.AppendLine(GetMethod_DataSource(dbname, viewName));
            sb.AppendLine();
            sb.AppendLine(GetMethod_Detail(dbname, viewName));
            sb.AppendLine();
            sb.AppendLine(GetMethod_Insert_Open(viewName));
            sb.AppendLine();
            sb.AppendLine(GetMethod_Insert_Commit(dbname, tableName));
            sb.AppendLine();
            sb.AppendLine(GetMethod_Update_Open(dbname, viewName));
            sb.AppendLine();
            sb.AppendLine(GetMethod_Update_Commit(dbname, tableName));
            sb.AppendLine();
            sb.AppendLine(GetMethod_Delete(dbname, tableName));
            sb.AppendLine();
            var controllerClassContent = sb.ToString();

            sb.Clear();
            sb.AppendLine(string.Format("using {0}.BusinessData;", solutionName));
            sb.AppendLine(string.Format("using {0}.BusinessAccess;", solutionName));
            sb.AppendLine("using Infoline.Web.Utility;");
            sb.AppendLine("using Kendo.Mvc.Extensions;");
            sb.AppendLine("using Kendo.Mvc.UI;");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Web.Mvc;");
            sb.AppendLine("");

            sb.AppendFormat("namespace {0}.Areas.{1}.Controllers\r\n", projectName, areaName);
            sb.AppendLine("{");
            sb.AppendFormat("\tpublic class {0}Controller : Controller\r\n", viewName);
            sb.AppendLine("\t{");
            sb.AppendLine(controllerClassContent);
            sb.AppendLine("\t}");
            sb.AppendLine("}");

            var controllerContent = sb.ToString();
            return controllerContent;
        }
        private string GetMethod_Constructer()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\tpublic ActionResult Index()");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t    return View();");
            sb.AppendLine("\t\t}");
            return sb.ToString();
        }
        private string GetMethod_DataSource(string dbname, string viewName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\tpublic JsonResult DataSource([DataSourceRequest]DataSourceRequest request)");
            sb.AppendLine("\t\t{");
            sb.AppendFormat("\t\t    var db = new {0}Database();\r\n", dbname);
            sb.AppendFormat("\t\t    var data = db.Get{0}().RemoveGeographies().ToDataSourceResult(request);\r\n", viewName);
            sb.AppendLine("\t\t    return Json(data, JsonRequestBehavior.AllowGet);");
            sb.AppendLine("\t\t}");
            return sb.ToString();
        }
        private string GetMethod_Insert_Open(string viewName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\tpublic ActionResult Insert()");
            sb.AppendLine("\t\t{");
            sb.AppendFormat("\t\t    var data = new {0} {{ id = Guid.NewGuid() }};\r\n", viewName);
            sb.AppendLine("\t\t    return View(data);");
            sb.AppendLine("\t\t}");
            return sb.ToString();
        }
        private string GetMethod_Insert_Commit(string dbname, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\t[HttpPost, ValidateAntiForgeryToken]");
            sb.AppendFormat("\t\tpublic JsonResult Insert({0} item)\r\n", tableName);
            sb.AppendLine("\t\t{");
            sb.AppendFormat("\t\t    var db = new {0}Database();\r\n", dbname);
            sb.AppendLine("\t\t    var feedback = new FeedBack();");
            sb.AppendLine("\t\t    item.created = DateTime.Now;");
            sb.AppendLine("\t\t    item.createdby = Guid.NewGuid();//bura security kısmı gelince düzenlenecek");
            sb.AppendFormat("\t\t    var dbresult = db.Insert{0}(item);\r\n", tableName);
            sb.AppendLine("\t\t    var result = new ResulStatusUI");
            sb.AppendLine("\t\t    {");
            sb.AppendLine("\t\t        Result = dbresult.result,");
            sb.AppendLine("\t\t        FeedBack = dbresult.result ? feedback.Success(\"Kaydetme işlemi başarılı\") : feedback.Error(\"Kaydetme işlemi başarısız\")");
            sb.AppendLine("\t\t    };");
            sb.AppendLine("\t\t");
            sb.AppendLine("\t\t    return Json(result, JsonRequestBehavior.AllowGet);");
            sb.AppendLine("\t\t}");
            return sb.ToString();
        }
        private string GetMethod_Update_Open(string dbname, string viewName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\tpublic ActionResult Update(Guid id)");
            sb.AppendLine("\t\t{");
            sb.AppendFormat("\t\t    var db = new {0}Database();\r\n", dbname);
            sb.AppendFormat("\t\t    var data = db.Get{0}ById(id);\r\n", viewName);
            sb.AppendLine("\t\t    return View(data);");
            sb.AppendLine("\t\t}");
            return sb.ToString();
        }
        private string GetMethod_Update_Commit(string dbname, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\t[HttpPost, ValidateAntiForgeryToken]");
            sb.AppendFormat("\t\tpublic JsonResult Update({0} item)\r\n", tableName);
            sb.AppendLine("\t\t{");
            sb.AppendFormat("\t\t    var db = new {0}Database();\r\n", dbname);
            sb.AppendLine("\t\t    var feedback = new FeedBack();");
            sb.AppendLine("\t\t");
            sb.AppendLine("\t\t    item.changed = DateTime.Now;");
            sb.AppendLine("\t\t    item.changedby = Guid.NewGuid();//bura security kısmı gelince düzenlenecek");
            sb.AppendLine("\t\t");
            sb.AppendFormat("\t\t    var dbresult = db.Update{0}(item);\r\n", tableName);
            sb.AppendLine("\t\t    var result = new ResulStatusUI");
            sb.AppendLine("\t\t    {");
            sb.AppendLine("\t\t        Result = dbresult.result,");
            sb.AppendLine("\t\t        FeedBack = dbresult.result ? feedback.Success(\"Güncelleme işlemi başarılı\") : feedback.Error(\"Güncelleme işlemi başarısız\")");
            sb.AppendLine("\t\t    };");
            sb.AppendLine("\t\t");
            sb.AppendLine("\t\t    return Json(result, JsonRequestBehavior.AllowGet);");
            sb.AppendLine("\t\t}");
            return sb.ToString();
        }
        private string GetMethod_Detail(string dbname, string viewName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\tpublic ActionResult Detail(Guid id)");
            sb.AppendLine("\t\t{");
            sb.AppendFormat("\t\t    var db = new {0}Database();\r\n", dbname);
            sb.AppendFormat("\t\t    var data = db.Get{0}ById(id);\r\n", viewName);
            sb.AppendLine("\t\t    return View(data);");
            sb.AppendLine("\t\t}");

            return sb.ToString();
        }
        private string GetMethod_Delete(string dbname, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\t[HttpPost]");
            sb.AppendLine("\t\tpublic JsonResult Delete(string[] id)");
            sb.AppendLine("\t\t{");
            sb.AppendFormat("\t\t    var db = new {0}Database();\r\n", dbname);
            sb.AppendLine("\t\t    var feedback = new FeedBack();");
            sb.AppendLine("\t\t");
            sb.AppendFormat("\t\t    var item = id.Select(a => new {0} {{ id = new Guid(a) }});\r\n", tableName);
            sb.AppendLine("\t\t");
            sb.AppendFormat("\t\t    var dbresult = db.BulkDelete{0}(item);\r\n", tableName);
            sb.AppendLine("\t\t");
            sb.AppendLine("\t\t    var result = new ResulStatusUI");
            sb.AppendLine("\t\t    {");
            sb.AppendLine("\t\t        Result = dbresult.result,");
            sb.AppendLine("\t\t        FeedBack = dbresult.result ? feedback.Success(\"Silme işlemi başarılı\") : feedback.Error(\"Silme işlemi başarılı\")");
            sb.AppendLine("\t\t    };");
            sb.AppendLine("\t\t");
            sb.AppendLine("\t\t    return Json(result, JsonRequestBehavior.AllowGet);");
            sb.AppendLine("\t\t}");
            return sb.ToString();
        }



        public Dictionary<string, string> GenerateViews(string strConn, string solutionName, object[] obj_tables)
        {
            var result = new Dictionary<string, string>();
            if (obj_tables == null) return result;
            var tables = obj_tables.Cast<MVCPageSetup>().ToArray();


            var con = new SqlConnection(strConn); con.Open();
            var dbname = con.Database;
            //var schemaTbl = con.GetSchema("Tables", new string[] { null, null, null, "VIEW" });
            //var rows = schemaTbl.Rows.Cast<DataRow>().Where(a => tables.Select(b => b.ViewName).Contains(a["TABLE_NAME"]));

            for (int i = 0; i < tables.Length; i++)
            {
                var table = tables[i];

                var views = GetViews(con, solutionName, table);
                foreach (var view in views)
                    result.Add(view.Key, view.Value);
            }

            return result;
        }

        private Dictionary<string, string> GetViews(SqlConnection con, string solutionName, MVCPageSetup page)
        {
            var result = new Dictionary<string, string>();
            var objArrRestrict = new string[] { null, null, page.ViewName, null };
            var columns = con.GetSchema("Columns", objArrRestrict);

            var tableName = page.TableName;
            var viewName = page.ViewName;
            var areaName = page.Area;
            var controllerLocation = string.Format("Areas/{0}/Controllers/{1}Controller.cs", areaName, viewName);
            var indexPageLocation = string.Format("Areas/{0}/Views/{1}/Index.cshtml", areaName, viewName);
            var deatilPageLocation = string.Format("Areas/{0}/Views/{1}/Detail.cshtml", areaName, viewName);
            var insertPageLocation = string.Format("Areas/{0}/Views/{1}/Insert.cshtml", areaName, viewName);
            var updatePageLocation = string.Format("Areas/{0}/Views/{1}/Update.cshtml", areaName, viewName);

            result.Add(indexPageLocation, GetPageIndex(solutionName, areaName, viewName, columns));
            result.Add(deatilPageLocation, GetPage_Detail(solutionName, viewName, columns));
            result.Add(insertPageLocation, GetPage_Insert(solutionName, viewName, columns));
            result.Add(updatePageLocation, GetPage_Update(solutionName, viewName, columns));

            return result;
        }
        private string GetPageIndex(string solutionName, string areaName, string viewName, DataTable columns)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@{");
            sb.AppendLine("    ViewBag.Title = \"Index\";");
            sb.AppendLine("    Layout = \"~/Views/Shared/_Layout.cshtml\";");
            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendFormat("<h2>{0}</h2>\r\n", viewName);
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("@(Html.Kendo()");
            sb.AppendFormat("      .Grid<{0}.BusinessData.{1}>()\r\n", solutionName, viewName);
            sb.AppendFormat("      .Name(\"{0}\")\r\n", viewName);
            sb.AppendFormat("      .DataSource(x => x.Ajax().Read(r => r.Action(\"DataSource\", \"{0}\", new {{ area = \"{1}\" }})).PageSize(25))\r\n", viewName, areaName);
            sb.AppendLine("      .Columns(x =>");
            sb.AppendLine("      {");
            var cols1 = columns.Rows.OfType<DataRow>().Where(a => ((string)a["column_name"]) == "id");
            var cols2 = columns.Rows.OfType<DataRow>().Where(a => ((string)a["column_name"]) != "id");
            var cols = cols1.Union(cols2);
            foreach (DataRow colrow in cols)
            {
                var cname = (string)colrow["column_name"];
                var dtype = (string)colrow["data_type"];
                sb.AppendFormat("          x.Bound(y => y.{0})", cname);
                if (cname != "id")
                {
                    sb.AppendFormat(".Title(\"{0}\")", cname);
                    sb.Append(".Width(100)");
                    if (dtype == "datetime")
                        sb.Append(".Format(\"{0:dd.MM.yyyy - HH:mm}\")");
                }
                else
                {
                    sb.Append(".Title(\"\")");
                    sb.Append(".ClientTemplate(\"<input type=\\\"checkbox\\\" data-event=\\\"GridSelector\\\" />\")");
                    sb.Append(".Width(20)");
                    sb.Append(".Sortable(false)");
                    sb.Append(".Filterable(false)");
                }
                sb.Append(";");
                sb.AppendLine();
            }
            sb.AppendLine("      })");
            sb.AppendLine("      .Selectable(x => x.Mode(GridSelectionMode.Multiple))");
            sb.AppendLine("      .Scrollable(builder => builder.Height(600))");
            sb.AppendLine("      .Sortable()");
            sb.AppendLine("      .Filterable()");
            sb.AppendLine("      .Navigatable()");
            sb.AppendLine("      .Pageable(x => x.PageSizes(new[] { 5, 10, 25, 50, 100, 250 }).Refresh(true))");
            sb.AppendLine("      .ToolBar(x =>");
            sb.AppendLine("      {");
            sb.AppendFormat("          x.Custom().Text(\"Ekle\").HtmlAttributes(new Dictionary<string, object>() {{ {{ \"data-task\", \"Insert\" }}, {{ \"data-modal\", \"true\" }}, {{ \"data-target\", Url.Action(\"Insert\", \"{0}\", new {{ area = \"{1}\" }}) }} }}).Url(\"#\");\r\n", viewName, areaName);
            sb.AppendFormat("          x.Custom().Text(\"Düzenle\").HtmlAttributes(new Dictionary<string, object>() {{ {{ \"data-task\", \"Update\" }}, {{ \"data-modal\", \"true\" }}, {{ \"data-target\", Url.Action(\"Update\", \"{0}\", new {{ area = \"{1}\" }}) }} }}).Url(\"#\");\r\n", viewName, areaName);
            sb.AppendFormat("          x.Custom().Text(\"Detay\").HtmlAttributes(new Dictionary<string, object>() {{ {{ \"data-task\", \"Detail\" }}, {{ \"data-modal\", \"true\" }}, {{ \"data-target\", Url.Action(\"Detail\", \"{0}\", new {{ area = \"{1}\" }}) }} }}).Url(\"#\");\r\n", viewName, areaName);
            sb.AppendFormat("          x.Custom().Text(\"Sil\").HtmlAttributes(new Dictionary<string, object>() {{ {{ \"data-task\", \"Delete\" }}, {{ \"data-modal\", \"true\" }}, {{ \"data-target\", Url.Action(\"Delete\", \"{0}\", new {{ area = \"{1}\" }}) }} }}).Url(\"#\");\r\n", viewName, areaName);
            sb.AppendLine("");
            sb.AppendLine("          x.Pdf().Text(\"PDF'e Aktar\");");
            sb.AppendLine("          x.Excel().Text(\"Excel'e Aktar\");");
            sb.AppendLine("      })");
            sb.AppendLine("      .Excel(x => x.FileName(Guid.NewGuid() + \".xlsx\"))");
            sb.AppendLine("      .Pdf(x => x.FileName(Guid.NewGuid() + \".pdf\").ProxyURL(\"http://demos.telerik.com/kendo-ui/service/export\"))");
            sb.AppendLine("      .Events(x =>");
            sb.AppendLine("      {");
            sb.AppendLine("          x.DataBound(\"LoadGridData\");");
            sb.AppendLine("          x.Change(\"GridChange\");");
            sb.AppendLine("      }))");
            return sb.ToString();
        }
        private string GetPage_Detail(string solutionName, string viewName, DataTable columns)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("@model {0}.BusinessData.{1}\r\n", solutionName, viewName);
            sb.AppendLine("@{");
            sb.AppendLine("    ViewBag.Title = \"Kayıt Detay\";");
            sb.AppendLine("    Layout = \"~/Views/Shared/_Layout.cshtml\";");
            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendLine("<div class=\"form-horizantal\" data-selector=\"modalContainer\">");

            var cols1 = columns.Rows.OfType<DataRow>().Where(a => ((string)a["column_name"]) == "id");
            var cols2 = columns.Rows.OfType<DataRow>().Where(a => ((string)a["column_name"]) != "id");
            var cols = cols1.Union(cols2);
            foreach (DataRow colrow in cols)
            {
                var cname = (string)colrow["column_name"];
                var dtype = (string)colrow["data_type"];

                sb.AppendLine("    <div class=\"form-group\">");
                sb.AppendLine("        <div class=\"col-md-4\">");
                sb.AppendFormat("            <label class=\"control-label label-md\" for=\"{0}\">{0}</label>\r\n", cname);
                sb.AppendLine("        </div>");
                sb.AppendLine("        <div class=\"col-md-8\">");
                sb.AppendFormat("            @Html.TextBoxFor(model => model.{0}, new Dictionary<string, object>() \r\n", cname);
                sb.AppendLine("            {");
                sb.AppendLine("                {\"class\", \"form-control\"},");
                sb.AppendFormat("                {{\"placeholder\", \"Lütfen {0} giriniz.\"}},\r\n", cname);
                sb.AppendLine("                {\"maxlength\",\"250\"},");
                sb.AppendLine("                {\"minlength\",\"2\"},");
                sb.AppendLine("                {\"required\", \"required\"}");
                sb.AppendLine("            })");
                sb.AppendLine("        </div>");
                sb.AppendLine("    </div>");
                sb.AppendLine("");
                sb.AppendLine("");
            }

            sb.AppendLine("    <div class=\"buttons\">");
            sb.AppendLine("        <button class=\"btn btn-md btn-danger pull-left\" data-task=\"modalClose\">Geri</button>");
            sb.AppendLine("    </div>");
            sb.AppendLine("");
            sb.AppendLine("</div>");
            return sb.ToString();
        }
        private string GetPage_Insert(string solutionName, string viewName, DataTable columns)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("@model {0}.BusinessData.{1}\r\n", solutionName, viewName);
            sb.AppendLine("@{");
            sb.AppendLine("    ViewBag.Title = \"Kayıt Ekleme\";");
            sb.AppendLine("    Layout = \"~/Views/Shared/_Layout.cshtml\";");
            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendFormat("@using(Html.BeginForm(\"Insert\", \"{0}\", FormMethod.Post, new Dictionary<string, object>() {{  \r\n", viewName);
            sb.AppendLine("    { \"class\", \"form-horizontal\" },");
            sb.AppendLine("    { \"role\", \"form\" },");
            sb.AppendLine("    { \"data-selector\", \"modalContainer\" },");
            sb.AppendLine("    { \"data-formType\", \"Ajax\" }");
            sb.AppendLine("}))");
            sb.AppendLine("{");
            sb.AppendLine("    @Html.AntiForgeryToken()");
            sb.AppendLine("    @Html.ValidationSummary(true)");
            sb.AppendLine("    @Html.HiddenFor(model => model.id)");
            sb.AppendLine("");
            sb.AppendLine("");


            var cols1 = columns.Rows.OfType<DataRow>().Where(a => ((string)a["column_name"]) == "id");
            var cols2 = columns.Rows.OfType<DataRow>().Where(a => ((string)a["column_name"]) != "id");
            var cols = cols1.Union(cols2);
            foreach (DataRow colrow in cols)
            {
                var cname = (string)colrow["column_name"];
                var dtype = (string)colrow["data_type"];

                sb.AppendLine("    <div class=\"form-group\">");
                sb.AppendLine("        <div class=\"col-md-4\">");
                sb.AppendFormat("            <label class=\"control-label label-md\" for=\"{0}\">{0}</label>\r\n", cname);
                sb.AppendLine("        </div>");
                sb.AppendLine("        <div class=\"col-md-8\">");
                sb.Append(GetRazorInputByType(cname, dtype));
                sb.AppendLine("        </div>");
                sb.AppendLine("    </div>");
                sb.AppendLine("");
                sb.AppendLine("");
            }

            sb.AppendLine("    <div class=\"buttons\">");
            sb.AppendLine("        <button class=\"btn btn-md btn-danger pull-left\" data-task=\"modalClose\">Geri</button>");
            sb.AppendLine("        <button class=\"btn btn-md btn-success pull-right\" type=\"submit\">Kaydet</button>");
            sb.AppendLine("    </div>");
            sb.AppendLine("");
            sb.AppendLine("}");
            return sb.ToString();
        }
        private string GetPage_Update(string solutionName, string viewName, DataTable columns)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("@model {0}.BusinessData.{1}\r\n", solutionName, viewName);
            sb.AppendLine("@{");
            sb.AppendLine("    ViewBag.Title = \"Kayıt Güncelleme\";");
            sb.AppendLine("    Layout = \"~/Views/Shared/_Layout.cshtml\";");
            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendFormat("@using(Html.BeginForm(\"Update\", \"{0}\", FormMethod.Post, new Dictionary<string, object>() {{  \r\n", viewName);
            sb.AppendLine("    { \"class\", \"form-horizontal\" },");
            sb.AppendLine("    { \"role\", \"form\" },");
            sb.AppendLine("    { \"data-selector\", \"modalContainer\" },");
            sb.AppendLine("    { \"data-formType\", \"Ajax\" }");
            sb.AppendLine("}))");
            sb.AppendLine("{");
            sb.AppendLine("    @Html.AntiForgeryToken()");
            sb.AppendLine("    @Html.ValidationSummary(true)");
            sb.AppendLine("    @Html.HiddenFor(model => model.id)");
            sb.AppendLine("");
            sb.AppendLine("");

            var cols1 = columns.Rows.OfType<DataRow>().Where(a => ((string)a["column_name"]) == "id");
            var cols2 = columns.Rows.OfType<DataRow>().Where(a => ((string)a["column_name"]) != "id");
            var cols = cols1.Union(cols2);
            foreach (DataRow colrow in cols)
            {
                var cname = (string)colrow["column_name"];
                var dtype = (string)colrow["data_type"];

                sb.AppendLine("    <div class=\"form-group\">");
                sb.AppendLine("        <div class=\"col-md-4\">");
                sb.AppendFormat("            <label class=\"control-label label-md\" for=\"{0}\">{0}</label>\r\n", cname);
                sb.AppendLine("        </div>");
                sb.AppendLine("        <div class=\"col-md-8\">");
                sb.Append(GetRazorInputByType(cname, dtype));
                sb.AppendLine("        </div>");
                sb.AppendLine("    </div>");
                sb.AppendLine("");
                sb.AppendLine("");
            }

            sb.AppendLine("    <div class=\"buttons\">");
            sb.AppendLine("        <button class=\"btn btn-md btn-danger pull-left\" data-task=\"modalClose\">Geri</button>");
            sb.AppendLine("        <button class=\"btn btn-md btn-success pull-right\" type=\"submit\">Kaydet</button>");
            sb.AppendLine("    </div>");
            sb.AppendLine("");
            sb.AppendLine("}");
            return sb.ToString();
        }



        private string GetRazorInputByType(string cname, string dtype)
        {
            StringBuilder sb = new StringBuilder();


            switch (dtype)
            {
                case "boolean":

                    sb.AppendLine("            @(Html.Kendo().CheckBoxFor(a=>a.Status).Checked(true))");

                    break;

                case "uniqueidentifier":
                    
                    sb.AppendFormat("          @(Html.Kendo().DropDownListFor(model => model.{0}).HtmlAttributes(new Dictionary<string, object>() \r\n", cname);
                    sb.AppendLine("            {");
                    sb.AppendLine("                {\"style\", \"width:100%\"},");
                    sb.AppendLine("                {\"class\", \"form-control\"},");
                    sb.AppendFormat("                {{\"placeholder\", \"Lütfen {0} giriniz.\"}},\r\n", cname);
                    sb.AppendLine("                {\"required\", \"required\"}");
                    sb.AppendLine("            })");
                    sb.AppendLine("            .OptionLabel(\"lütfen seçim yapınız\")");
                    sb.AppendLine("            .DataTextField(\"Name\")");
                    sb.AppendLine("            .DataValueField(\"Id\")");
                    sb.AppendLine("            .DataSource(s =>");
                    sb.AppendLine("            {");
                    sb.AppendLine("                 s.Read(r =>");
                    sb.AppendLine("                 {");
                    sb.AppendLine("                     r.Action(\"\", \"General\", new { area = string.Empty }); //General controllerının içine datasının oldugu metodu yaz ilk parametreye method ismini ver");
                    sb.AppendLine("                 });");
                    sb.AppendLine("             })");
                    sb.AppendLine("             )");

                    break;

                case "datetime":


                    sb.AppendFormat("          @(Html.Kendo().DatePickerFor(model => model.{0}).HtmlAttributes(new Dictionary<string, object>() \r\n", cname);
                    sb.AppendLine("            {");
                    sb.AppendLine("                {\"class\", \"form-control\"},");
                    sb.AppendFormat("                {{\"placeholder\", \"Lütfen {0} giriniz.\"}},\r\n", cname);
                    sb.AppendLine("                {\"required\", \"required\"}");
                    sb.AppendLine("            }).Format(\"dd.MM.yyyy\"))");

                    break;
                default:

                    sb.AppendFormat("            @Html.TextBoxFor(model => model.{0}, new Dictionary<string, object>() \r\n", cname);
                    sb.AppendLine("            {");
                    sb.AppendLine("                {\"class\", \"form-control\"},");
                    sb.AppendFormat("                {{\"placeholder\", \"Lütfen {0} giriniz.\"}},\r\n", cname);
                    sb.AppendLine("                {\"maxlength\",\"250\"},");
                    sb.AppendLine("                {\"minlength\",\"2\"},");
                    sb.AppendLine("                {\"required\", \"required\"}");
                    sb.AppendLine("            })");

                    break;
            }



            return sb.ToString();
        }

        private string GetAreaRegistration(string projectName, string areaName)
        {

            var str = @"using System.Web.Mvc;

namespace {0}.Areas.{1}
{{
    public class {1}AreaRegistration : AreaRegistration
    {{
        public override string AreaName
        {{
            get
            {{
                return ""{1}"";
            }}
        }}

        public override void RegisterArea(AreaRegistrationContext context)
        {{
            context.MapRoute(
                ""{1}_default"",
                ""{1}/{{controller}}/{{action}}/{{id}}"",
                new {{ action = ""Index"", id = UrlParameter.Optional }}
            );
        }}
    }}
}}";

            return string.Format(str, projectName, areaName);
        }
        private string GetWebConfig()
        {
            var str = @"<?xml version=""1.0""?>

<configuration>
  <configSections>
    <sectionGroup name=""system.web.webPages.razor"" type=""System.Web.WebPages.Razor.Configuration.RazorWebSectionGroup, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"">
      <section name=""host"" type=""System.Web.WebPages.Razor.Configuration.HostSection, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"" requirePermission=""false"" />
      <section name=""pages"" type=""System.Web.WebPages.Razor.Configuration.RazorPagesSection, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"" requirePermission=""false"" />
    </sectionGroup>
  </configSections>

  <system.web.webPages.razor>
    <host factoryType=""System.Web.Mvc.MvcWebRazorHostFactory, System.Web.Mvc, Version=5.2.3.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35"" />
    <pages pageBaseType=""System.Web.Mvc.WebViewPage"">
      <namespaces>
        <add namespace=""System.Web.Mvc"" />
        <add namespace=""System.Web.Mvc.Ajax"" />
        <add namespace=""System.Web.Mvc.Html"" />
        <add namespace=""System.Web.Routing"" />
        <add namespace=""System.Web.Optimization"" />
        <add namespace=""Kendo.Mvc.UI"" />
        <add namespace=""Infoline.TarimBilgiSistemi.WebProject"" />
      </namespaces>
    </pages>
  </system.web.webPages.razor>

  <appSettings>
    <add key=""webpages:Enabled"" value=""false"" />
  </appSettings>

  <system.webServer>
    <handlers>
      <remove name=""BlockViewHandler""/>
      <add name=""BlockViewHandler"" path=""*"" verb=""*"" preCondition=""integratedMode"" type=""System.Web.HttpNotFoundHandler"" />
    </handlers>
  </system.webServer>
</configuration>";
            return str;
        }

    }
}
