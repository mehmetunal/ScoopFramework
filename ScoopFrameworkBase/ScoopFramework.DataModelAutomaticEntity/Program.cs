using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ScoopFramework.DataLayer;
using ScoopFramework.DataLayer.OldSource;
using ScoopFramework.DataModelAutomaticEntity.Model;
using ScoopFramework.DataModelAutomaticEntity.DataBaseType;

namespace ScoopFramework.DataModelAutomaticEntity
{
    class Program
    {
        private static string _solutionPath;
        private static string _tempSolutionPath;
        private static string _projectName;

        static void Main(string[] args)
        {
            _tempSolutionPath = Application.StartupPath;
            _solutionPath = _tempSolutionPath.Replace("\\bin\\Debug", "\\EntityModel\\");
            _projectName = Application.ProductName;

            const string sqlText = @"SELECT c.TABLE_NAME, c.COLUMN_NAME,c.DATA_TYPE
             ,CASE WHEN pk.COLUMN_NAME IS NOT NULL THEN 'PRIMARY KEY' ELSE '' END AS KEY_TYPE
FROM INFORMATION_SCHEMA.COLUMNS c
LEFT JOIN (
            SELECT ku.TABLE_CATALOG,ku.TABLE_SCHEMA,ku.TABLE_NAME,ku.COLUMN_NAME
            FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS tc
            INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS ku
                ON tc.CONSTRAINT_TYPE = 'PRIMARY KEY' 
                AND tc.CONSTRAINT_NAME = ku.CONSTRAINT_NAME
         )   pk 
ON  c.TABLE_CATALOG = pk.TABLE_CATALOG
            AND c.TABLE_SCHEMA = pk.TABLE_SCHEMA
            AND c.TABLE_NAME = pk.TABLE_NAME
            AND c.COLUMN_NAME = pk.COLUMN_NAME
ORDER BY c.TABLE_SCHEMA,c.TABLE_NAME, c.ORDINAL_POSITION ";

            List<DataTableColunmEntity> dataTableColunmEntities = DataAccessLayer<DataTableColunmEntity>.ScoopSqlCommand(sqlText);
            StreamWriter sw = null;
            var filePathEntity = _tempSolutionPath.Replace("\\bin\\Debug", "\\");
            Directory.CreateDirectory(@"" + filePathEntity + "EntityModel");
            var tmpTabloName = string.Empty;
            var flag = false;
            var tmpClass = string.Empty;

            foreach (var item in dataTableColunmEntities)
            {
                if (!flag)
                {
                    tmpTabloName = item.TABLE_NAME;
                    sw = File.CreateText(@"" + _solutionPath + item.TABLE_NAME + ".cs");
                }
                if (tmpTabloName == item.TABLE_NAME)
                {
                    tmpClass =
                        @" using System; " + Environment.NewLine + "namespace "
                        + _projectName + ".EntityModel" + "" + Environment.NewLine
                        + "{ public class " + item.TABLE_NAME + "{" + Environment.NewLine;

                    string datatTableColumn = DatatTableColumn(item.TABLE_NAME, dataTableColunmEntities);
                    tmpClass += datatTableColumn;
                    flag = true;

                }
                sw.WriteLine(tmpClass);
                sw.Close();
                tmpClass = string.Empty;
                flag = false;
            }
            Console.WriteLine(@"Entity Classları oluşturuldu");
            Console.ReadLine();
            //Log.LogItem("Form", "Entity Classları oluşturuldu");
        }

        private static string DatatTableColumn(string param, IEnumerable<DataTableColunmEntity> value)
        {
            var dataValueType = new DataValueType();
            var tmpClass = string.Empty;
            foreach (var item in value)
            {
                if (!string.IsNullOrEmpty(item.KEY_TYPE))
                {
                    if (param == item.TABLE_NAME)
                        tmpClass = tmpClass + Environment.NewLine +
                                   (@"[PrimaryKey]" + Environment.NewLine +
                                      "public " + dataValueType.ColumnTypes(item.DATA_TYPE == "date" ? "DateTime" : item.DATA_TYPE) + " " + item.COLUMN_NAME + " { get; set; }" + Environment.NewLine);
                }
                else
                {
                    if (param == item.TABLE_NAME)
                        tmpClass = tmpClass + Environment.NewLine +
                            (@"public " + dataValueType.ColumnTypes(item.DATA_TYPE == "date" ? "DateTime" : item.DATA_TYPE) + " " + item.COLUMN_NAME + " { get; set; }" + Environment.NewLine);

                }

                //Log.LogItem(item.TABLE_NAME, "Entity Classları oluşturuldu");
            }
            tmpClass += @"" + Environment.NewLine +
                            "}" + Environment.NewLine +
                       "}";
            return tmpClass;
        }
    }
}
