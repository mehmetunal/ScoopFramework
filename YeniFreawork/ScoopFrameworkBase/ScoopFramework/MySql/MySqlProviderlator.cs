using MySql.Data.MySqlClient;
using ScoopFramework.Helper;
using ScoopFramework.Log;
using ScoopFramework.Model;
using ScoopFramework.MySql.Interface;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ScoopFramework.MySql
{
    public class MySqlProviderlator : IMysqlProvider
    {
        MySqlConnection _connection;
        public MySqlProviderlator(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
        }

        public List<T> GetList<T>(string sql, List<MySqlParameter> parameter = null)
        {
            try
            {
                using (var command = prepareCommand(sql, parameter))
                {
                    var table = new DataTable();
                    using (var daptor = new MySqlDataAdapter())
                    {
                        daptor.SelectCommand = command;
                        daptor.Fill(table);
                    }
                    return table.ToConvertDateTableForList<T>();
                }
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        public T GetFirstOrDefault<T>(string sql, List<MySqlParameter> parameter = null)
        {
            try
            {
                using (var command = prepareCommand(sql, parameter))
                {
                    var table = new DataTable();
                    using (var daptor = new MySqlDataAdapter())
                    {
                        daptor.SelectCommand = command;
                        daptor.Fill(table);
                    }
                    return table.ToConvertDateTableForList<T>().FirstOrDefault();
                }
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        public DataTable GetDataTable(string sql, List<MySqlParameter> parameter = null)
        {
            try
            {
                using (var command = prepareCommand(sql, parameter))
                {
                    var table = new DataTable();
                    using (var daptor = new MySqlDataAdapter())
                    {
                        daptor.SelectCommand = command;
                        daptor.Fill(table);
                    }
                    return table;
                }
            }
            finally
            {
                _connection.Close();
                //_connection.Dispose();
            }
        }

        public MySqlDataReader GetReader(string sql, List<MySqlParameter> parameter = null)
        {
            try
            {
                using (var command = prepareCommand(sql, parameter))
                    return command.ExecuteReader();
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        public int Execute(string sql, List<MySqlParameter> parameter = null, bool connectionClose = true)
        {
            try
            {
                using (var command = prepareCommand(sql, parameter))
                {
                    return command.ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                new Logger().LogProvider.Error(ex.Message, "Execute");
                return (int)ReturnNoneQuery.EnumExecuteNonQuery.hatali;
            }
            finally
            {
                if (connectionClose)
                {
                    _connection.Close();
                    _connection.Dispose();
                }
            }
        }

        public int ExecuteScalar(string sql, List<MySqlParameter> parameter = null)
        {
            try
            {
                using (var command = prepareCommand(sql, parameter))
                {
                   return int.Parse(command.ExecuteScalar().ToString());
                }
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
        }
        
        MySqlCommand prepareCommand(string sql, List<MySqlParameter> parameter = null)
        {

            using (var command = new MySqlCommand(sql, _connection))
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }

                if (parameter == null) return command;

                command.Parameters.AddRange(parameter.ToArray());
                return command;
            }

        }
        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

    }
}