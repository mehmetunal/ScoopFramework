using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace ScoopFramework.MySql.Interface
{
    public interface IMysqlProvider : IDisposable
    {
        //CellSet GetCellSet(string sql, List<SqlParameter> parameter =null);
        List<T> GetList<T>(string sql, List<MySqlParameter> parameter = null);
        T GetFirstOrDefault<T>(string sql, List<MySqlParameter> parameter = null);
        DataTable GetDataTable(string sql, List<MySqlParameter> parameter = null);
        MySqlDataReader GetReader(string sql, List<MySqlParameter> parameter = null);
        int Execute(string sql, List<MySqlParameter> parameter = null, bool connectionClose = true);
        int ExecuteScalar(string sql, List<MySqlParameter> parameter = null);
    }
}
