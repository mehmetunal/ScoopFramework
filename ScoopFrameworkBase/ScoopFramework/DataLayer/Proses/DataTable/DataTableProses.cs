using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ScoopFramework.DataConnection;
using ScoopFramework.Model;

namespace ScoopFramework.DataLayer.Proses.DataTable
{
    public struct DataTableProses
    {
        public static System.Data.DataTable DataTableFill(List<ParamValue> paramValues, string sql, SqlConnection connection)
        {
            var dataTable = new System.Data.DataTable();

            using (var adapter = new SqlDataAdapter(sql, connection))
            {
                connection.Open();
                if (paramValues.Any(value => value.Where != null))
                {
                    foreach (var item in paramValues)
                    {
                        foreach (var row in item.Where)
                        {
                            var prm = string.Format("@{0}", row.Key.Replace("<", " ").Replace(">", " "));
                            adapter.SelectCommand.Parameters.AddWithValue(prm, row.Value);
                        }
                    }
                }
                adapter.Fill(dataTable);
            }
            return dataTable;
        }

        public static System.Data.DataTable GetDataTableSqlDataAdapter(string sql, SqlParameter[] parameter,
            System.Data.DataTable dataTable)
        {
            using (var connection = DBHelper.Connection())
            {
                using (var adapter = new SqlDataAdapter(sql, connection))
                {
                    if (parameter != null)
                    {
                        foreach (var sqlParameter in parameter)
                        {
                            adapter.SelectCommand.Parameters.AddWithValue(sqlParameter.ParameterName, sqlParameter.Value);
                        }
                    }
                    connection.Open();

                    adapter.Fill(dataTable);
                }
                connection.Close();
            }

            return dataTable;
        }
    }
}
