using ScoopFramework.Linq;
using ScoopFramework.Log;
using ScoopFramework.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace ScoopFramework.Helper
{

    public class Providerlator : IScoopProvider
    {
        private SqlConnection _connection;
        private SqlTransaction _transaction;

        public bool IsTransactionOpen => _transaction != null && _transaction.Connection != null;

        public string ConnectionString => _connection.ConnectionString;

        public string ServerName => _connection.DataSource;

        public string DbName => _connection.Database;

        public Providerlator(string connectionString, DbTransaction transaction = null)
        {
            _transaction = (SqlTransaction)transaction;
            _connection = _transaction == null || _transaction.Connection == null ? new SqlConnection(connectionString) : _transaction.Connection;
        }

        public SqlDataReader GetReader(string sql, List<SqlParameter> parameter)
        {
            try
            {
                using (var command = prepareCommand(sql, parameter))
                    return command.ExecuteReader();
            }
            finally
            {
                //_connection.Close();
                //_connection.Dispose();
            }
        }

        public int Execute(string sql, List<SqlParameter> parameter = null)
        {
            try
            {
                using (var command = prepareCommand(sql, parameter))
                {
                    var executeNonQuery = command.ExecuteNonQuery();
                    return executeNonQuery;
                }
            }
            catch (System.Exception ex)
            {
                new Logger().LogProvider.Error(ex.Message, "Execute");
                return (int)ReturnNoneQuery.EnumExecuteNonQuery.hatali;
            }
            finally
            {
                //if (connectionClose)
                //{
                //    _connection.Close();
                //    _connection.Dispose();
                //}
            }

        }

        public int ExecuteScalar(string sql, List<SqlParameter> parameter = null)
        {
            try
            {
                using (var command = prepareCommand(sql, parameter))
                {
                    return (int)command.ExecuteScalar();
                }
            }
            finally
            {
                //_connection.Close();
                //_connection.Dispose();
            }
        }

        public int BulkExecute(string tableName, DataTable dataTable)
        {

            try
            {
                using (var copy = new SqlBulkCopy(_connection, SqlBulkCopyOptions.KeepIdentity, _transaction))
                {
                    foreach (var item in dataTable.Columns)
                    {
                        if (item != null)
                        {
                            copy.ColumnMappings.Add(item.ToString(), item.ToString());
                        }
                    }

                    var value = new SqlBulkCopyColumnMapping("rowguid", "rowguid");
                    copy.ColumnMappings.Remove(value);

                    if (_connection.State != ConnectionState.Open)
                    {
                        _connection.Open();
                    }

                    copy.DestinationTableName = tableName;
                    copy.WriteToServer(dataTable);
                    //if (connectionClose)
                    //{
                    //    _connection.Close();
                    //    _connection.Dispose();
                    //}
                    return 1;
                }
            }
            catch (System.Exception exception)
            {

                return 0;
            }
            finally
            {
                //if (connectionClose)
                //{
                //    _connection.Close();
                //    _connection.Dispose();
                //}
            }

        }

        public int CreatedTable(string table)
        {
            try
            {
                var dbs = new DBScripter(_connection);
                var script = dbs.GetTableScript(table);
                if (!string.IsNullOrEmpty(script))
                {
                    script = script.Replace(table, "Temp" + table);
                }
                return Execute(script, null);
            }
            catch(System.Exception ex)
            {
                return 0;
            }
            finally
            {
                //if (connectionClose == true)
                //{
                //    _connection.Close();
                //    _connection.Dispose();
                //}
            }
        }


        public List<T> GetList<T>(string sql, List<SqlParameter> parameter)
        {
            try
            {
                using (var command = prepareCommand(sql, parameter))
                {
                    var table = new DataTable();
                    using (var daptor = new SqlDataAdapter())
                    {
                        daptor.SelectCommand = command;
                        daptor.Fill(table);
                    }
                    return table.ToConvertDateTableForList<T>();
                }
            }
            finally
            {
                //_connection.Close();
                //_connection.Dispose();
            }
        }

        public T GetFirstOrDefault<T>(string sql, List<SqlParameter> parameter = null)
        {
            try
            {
                using (var command = prepareCommand(sql, parameter))
                {
                    var table = new DataTable();
                    using (var daptor = new SqlDataAdapter())
                    {
                        daptor.SelectCommand = command;
                        daptor.Fill(table);
                    }
                    return table.ToConvertDateTableForList<T>().FirstOrDefault();
                }
            }
            finally
            {
                //_connection.Close();
                //_connection.Dispose();
            }
        }


        public DataTable GetDataTable(string sql, List<SqlParameter> parameter = null)
        {
            try
            {
                using (var command = prepareCommand(sql, parameter))
                {
                    var table = new DataTable();
                    using (var daptor = new SqlDataAdapter())
                    {
                        daptor.SelectCommand = command;
                        daptor.Fill(table);
                    }
                    return table;
                }
            }
            finally
            {
                //_connection.Close();
                //_connection.Dispose();
            }
        }
        SqlCommand prepareCommand(string sql, List<SqlParameter> parameter = null)
        {

            using (var command = new SqlCommand(sql, _connection))
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }

                if (_transaction != null && _transaction.Connection != null)
                    command.Transaction = _transaction;

                if (parameter == null) return command;

                command.Parameters.AddRange(parameter.ToArray());
                return command;
            }

        }

        public void ConnecTionStringControl()
        {
            if (string.IsNullOrEmpty(_connection.ConnectionString))
            {
                _connection = new SqlConnection(this.ConnectionString);
            }
        }

        public void Dispose()
        {
            if (_transaction == null || _transaction.Connection == null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }

        public void Close()
        {
            if (_connection.State != ConnectionState.Closed)
                _connection.Close();
        }

        public DbTransaction BeginTransaction()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
            _transaction = _connection.BeginTransaction();
            return _transaction;
        }
    }
}
