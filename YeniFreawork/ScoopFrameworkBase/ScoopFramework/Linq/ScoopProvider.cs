using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace ScoopFramework.Linq
{
    public interface IScoopProvider : IDisposable
    {
        string ConnectionString { get; }

        bool IsTransactionOpen { get; }
        string ServerName { get; }
        string DbName { get; }

        //CellSet GetCellSet(string sql, List<SqlParameter> parameter =null);
        List<T> GetList<T>(string sql, List<SqlParameter> parameter = null);
        T GetFirstOrDefault<T>(string sql, List<SqlParameter> parameter = null);
        DataTable GetDataTable(string sql, List<SqlParameter> parameter = null);
        SqlDataReader GetReader(string sql, List<SqlParameter> parameter = null);

        int Execute(string sql, List<SqlParameter> parameter = null);
        int ExecuteScalar(string sql, List<SqlParameter> parameter = null);
        int BulkExecute(string tableName, DataTable dataTable);
        int CreatedTable(string sqlQuery);

        DbTransaction BeginTransaction();
    }
}