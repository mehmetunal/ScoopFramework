using MySql.Data.MySqlClient;
using ScoopFramework.DataTables;
using ScoopFramework.Exception;
using ScoopFramework.Helper;
using ScoopFramework.Linq;
using ScoopFramework.Log;
using ScoopFramework.Mapping;
using ScoopFramework.Model;
using ScoopFramework.MySql.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace ScoopFramework.MySql
{
    public class ScoopMySql<T> : IMySqlQueryable<T>
    {
        private readonly IMysqlProvider _provider;
        private readonly List<ScoopComponent> _components;
        private readonly List<Axis<T>> _axisGroups;
        public IMysqlProvider Provider => _provider;

        public ScoopMySql(IMysqlProvider provider = null)
        {
            provider = null;//new ScoopDatabase().MySqlProviderConnection();
            _provider = provider;
            _components = new List<ScoopComponent>();
            _axisGroups = new List<Axis<T>>();
        }

        public IMySqlQueryable<T> OrderBy(Expression<Func<T, object>> solid)
        {
            var comp = new ScoopComponent(Component.OrderBy) { Creator = solid };
            _components.Add(comp);
            return this;
        }

        public IMySqlQueryable<T> OrderByDesc(Expression<Func<T, object>> solid)
        {
            var comp = new ScoopComponent(Component.OrderByDesc) { Creator = solid };
            _components.Add(comp);
            return this;
        }

        public IMySqlQueryable<T> Paggin(int page, int pageCount)
        {
            var comp = new ScoopComponent(Component.Paggin) { ScoopPagging = new ScoopPagging() { Page = page, PageCount = pageCount } };
            _components.Add(comp);
            return this;
        }

        public IMySqlQueryable<T> Count()
        {
            var comp = new ScoopComponent(Component.Count);
            _components.Add(comp);
            return this;
        }

        public IMySqlQueryable<T> Filtre(T entity)
        {
            var comp = new ScoopComponent(Component.Filtre) { Entity = entity };
            _components.Add(comp);
            return this;
        }

        public IMySqlQueryable<T> DataTablesFiltre(IDataTablesRequest dataTablesRequest)
        {
            var comp = new ScoopComponent(Component.DataTablesRequest) { DataTablesRequest = dataTablesRequest };
            _components.Add(comp);
            return this;
        }

        public IList<T> ExecuteReader(string sqlCommand, params MySqlParameter[] parametre)
        {
            return _provider.GetList<T>(sqlCommand, parametre.ToList());
        }

        public int ExecuteScalar(string sqlCommand, params MySqlParameter[] parametre)
        {
            return _provider.ExecuteScalar(sqlCommand, parametre.ToList());
        }

        public T FirstOrDefault()
        {
            var lator = new IPercolator<T>(_components);
            var command = lator.ScoopCommand;
            return _provider.GetFirstOrDefault<T>(command, lator._parameter);
        }

        public List<T> RunToList()
        {
            var lator = new IPercolator<T>(_components);
            var command = lator.ScoopCommand;
            return _provider.GetList<T>(command, lator._parameter);
        }

        public int RunCount()
        {
            var lator = new IPercolator<T>(_components);
            var command = lator.ScoopCommand;
            return _provider.ExecuteScalar(command, lator._parameter);
        }


        public ReturnValue Insert<TSource>(TSource entity) where TSource : new()
        {
            try
            {
                var baseName = new[] { "Object", "BaseEntity" };
                Type entityType;
                if (baseName.Contains(entity.GetType().BaseType.Name))
                {
                    entityType = entity.GetType();
                }
                else
                {
                    entityType = entity.GetType().BaseType;
                }

                var propery = entityType.GetProperties();

                var paramters = GetSqlParameters(entity, propery);

                var commandText = $"INSERT INTO {entityType.Name} ({paramters.paramKey.TrimEnd(',')}) VALUES ({paramters.paramValue.TrimEnd(',')})";

                var exeRun = _provider.Execute(commandText, paramters.MySqlParameter);

                return exeRun != (int)ReturnNoneQuery.EnumExecuteNonQuery.hatali
                    ? new ReturnValue() { Id = null, Mesaj = Comment.DB_MESSAGE_SUCCESS, Success = true }
                    : new ReturnValue() { Id = null, Mesaj = Comment.DB_MESSAGE_ERROR, Success = false };
            }
            catch (System.Exception ex)
            {
                if (HttpContext.Current == null)
                {
                    return new ReturnValue() { Id = null, Mesaj = ex.Message, Success = false };
                }
                new Logger().LogProvider.Error(ex.Message, "Insert Exception");
                return new ReturnValue() { Id = null, Mesaj = ex.Message, Success = false };
            }
        }

        public ReturnValue Update<TSource>(TSource entity, Expression<Func<TSource, object>> slicer)
        {
            try
            {
                var baseName = new[] { "Object", "BaseEntity" };
                Type entityType;
                if (baseName.Contains(entity.GetType().BaseType.Name))
                {
                    entityType = entity.GetType();
                }
                else
                {
                    entityType = entity.GetType().BaseType;
                }

                var propery = entityType.GetProperties();

                var paramters = GetSqlParameters(entity, propery);

                var commandText = $"UPDATE {entityType.Name} SET {paramters.updateParameters.TrimEnd(',')} ";

                var obj = slicer.GetValueWhere<T>();
                commandText += $" WHERE {obj}";

                var exeRun = _provider.Execute(commandText, paramters.MySqlParameter);

                return exeRun != (int)ReturnNoneQuery.EnumExecuteNonQuery.hatali
                    ? new ReturnValue() { Id = null, Mesaj = Comment.DB_MESSAGE_SUCCESS, Success = true }
                    : new ReturnValue() { Id = null, Mesaj = Comment.DB_MESSAGE_ERROR, Success = false };
            }
            catch (System.Exception ex)
            {
                if (HttpContext.Current == null)
                {
                    return new ReturnValue() { Id = null, Mesaj = ex.Message, Success = false };
                }
                new Logger().LogProvider.Error(ex.Message, "Update Exception");
                return new ReturnValue() { Id = null, Mesaj = ex.Message, Success = false };
            }
        }

        public ReturnValue Update<TSource>(TSource entity)
        {
            try
            {
                var baseName = new[] { "Object", "BaseEntity" };
                Type entityType;
                if (baseName.Contains(entity.GetType().BaseType.Name))
                {
                    entityType = entity.GetType();
                }
                else
                {
                    entityType = entity.GetType().BaseType;
                }

                var propery = entityType.GetProperties();

                var primaryKey = GetPrimaryKey(entityType.Name);

                var paramters = GetSqlParameters(entity, propery, primaryKey);

                var commandText =
                    $"UPDATE {entityType.Name} SET {paramters.updateParameters.TrimEnd(',')} ";

                #region PrimaryKey
                if (primaryKey != null)
                {
                    var value = entityType.GetProperty(primaryKey.ToString()).GetValue(entity, null);
                    var prmName = "@" + primaryKey;
                    commandText += $" WHERE {primaryKey}={prmName}";
                    paramters.Parameters.Add(new SqlParameter() { ParameterName = prmName, Value = value });
                }
                #endregion

                var exeRun = _provider.Execute(commandText, paramters.MySqlParameter);

                return exeRun != (int)ReturnNoneQuery.EnumExecuteNonQuery.hatali
                    ? new ReturnValue() { Id = null, Mesaj = Comment.DB_MESSAGE_SUCCESS, Success = true }
                    : new ReturnValue() { Id = null, Mesaj = Comment.DB_MESSAGE_ERROR, Success = false };
            }
            catch (System.Exception ex)
            {
                if (HttpContext.Current == null)
                {
                    return new ReturnValue() { Id = null, Mesaj = ex.Message, Success = false };
                }
                new Logger().LogProvider.Error(ex.Message, "Update Exception");
                return new ReturnValue() { Id = null, Mesaj = ex.Message, Success = false };
            }
        }

        public ReturnValue Delete<TSource>(TSource entity)
        {
            try
            {
                var baseName = new[] { "Object", "BaseEntity" };
                Type entityType;
                if (baseName.Contains(entity.GetType().BaseType.Name))
                {
                    entityType = entity.GetType();
                }
                else
                {
                    entityType = entity.GetType().BaseType;
                }

                var primaryKey = GetPrimaryKey(entityType.Name);

                var listeParam = new List<MySqlParameter>();

                var commandText = $"DELETE FROM  {entityType.Name} ";

                #region PrimaryKey
                if (primaryKey != null)
                {
                    var value = entityType.GetProperty(primaryKey.ToString()).GetValue(entity, null);

                    var prmName = "@" + primaryKey;
                    commandText += $" WHERE {primaryKey}={prmName}";

                    listeParam.Add(new MySqlParameter() { ParameterName = prmName, Value = value });
                }
                #endregion


                var exeRun = _provider.Execute(commandText, listeParam);

                return exeRun != (int)ReturnNoneQuery.EnumExecuteNonQuery.hatali
                    ? new ReturnValue() { Id = null, Mesaj = Comment.DB_MESSAGE_SUCCESS, Success = true }
                    : new ReturnValue() { Id = null, Mesaj = Comment.DB_MESSAGE_ERROR, Success = false };
            }
            catch (System.Exception ex)
            {
                if (HttpContext.Current == null)
                {
                    return new ReturnValue() { Id = null, Mesaj = ex.Message, Success = false };
                }
                new Logger().LogProvider.Error(ex.Message, "Delete Exception");
                return new ReturnValue() { Id = null, Mesaj = ex.Message, Success = false };
            }
        }
        public ReturnValue Delete<TSource>(TSource entity, Expression<Func<TSource, object>> slicer)
        {
            try
            {
                var sqlParameterList = new List<MySqlParameter>();

                var tableName = typeof(TSource).Name;

                var commandText = $"DELETE FROM  {tableName} ";

                var obj = slicer.GetDynamicMySqlWhere<T>(sqlParameterList);

                commandText += $" WHERE {obj}";

                var exeRun = _provider.Execute(commandText, sqlParameterList);

                return exeRun != (int)ReturnNoneQuery.EnumExecuteNonQuery.hatali
                    ? new ReturnValue() { Id = null, Mesaj = Comment.DB_MESSAGE_SUCCESS, Success = true }
                    : new ReturnValue() { Id = null, Mesaj = Comment.DB_MESSAGE_ERROR, Success = false };
            }
            catch (System.Exception ex)
            {
                if (HttpContext.Current == null)
                {
                    return new ReturnValue() { Id = null, Mesaj = ex.Message, Success = false };
                }
                new Logger().LogProvider.Error(ex.Message, "Delete Exception");
                return new ReturnValue() { Id = null, Mesaj = ex.Message, Success = false };
            }
        }

        public string TranslateToScoop()
        {
            return new IPercolator<T>(_components).ScoopCommand;
        }

        private object GetPrimaryKey(string tableName)
        {
            return null;
            //var getPrimaryKey = $"SELECT COLUMN_NAME FROM {DataBussiens.ScoopDatabase.ScoopDBShema.DbName}.INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME LIKE '{tableName}' ";
            //var dataTable = _provider.GetDataTable(getPrimaryKey);
            //return dataTable.Rows.Count > 0 ? dataTable.Rows[0][0] : null;
        }

        private static ReturnParameters GetSqlParameters<TSource>(TSource entity, PropertyInfo[] propery, object pk = null)
        {
            var paramters = new ReturnParameters();
            foreach (var propertyInfo in propery.Where(x => x.GetValue(entity, null) != null))
            {
                if (propertyInfo.GetValue(entity, null) == null)
                    continue;

                if (pk != null && (string)pk == propertyInfo.Name)
                    continue;

                if (((
                    propertyInfo.PropertyType == typeof(String) ||
                    propertyInfo.PropertyType == typeof(string) ||
                    propertyInfo.PropertyType == typeof(DateTime) ||
                    propertyInfo.PropertyType == typeof(DateTime?) ||
                    propertyInfo.PropertyType == typeof(Guid?) ||
                    propertyInfo.PropertyType == typeof(Guid)) &&
                    propertyInfo.GetValue(entity, null).Equals(propertyInfo.PropertyType.GetDefault())) ||
                    propertyInfo.GetValue(entity, null) == null)
                {
                    continue;
                }
                string prmName;
                if (paramters.Parameters.Any(x => x.ParameterName == "@" + propertyInfo.Name))
                    prmName = "@" + propertyInfo.Name + "1";
                else
                    prmName = "@" + propertyInfo.Name + "";

                paramters.paramKey += "" + propertyInfo.Name + ",";
                paramters.paramValue += prmName + ",";

                paramters.updateParameters += $"{"" + propertyInfo.Name + ""}={prmName},";

                paramters.MySqlParameter.Add(new MySqlParameter()
                {
                    ParameterName = prmName,
                    Value = propertyInfo.GetValue(entity, null)
                });
            }
            return paramters;
        }
        public override string ToString()
        {
            return TranslateToScoop();
        }

        public IMySqlQueryable<T> OnAxis(byte axisNumber, Expression<Func<T, object>> axisObjects)
        {
            if (axisNumber > 127)
                throw new PercolatorException("Axis max is 128");
            return OnAxis(axisNumber, false, axisObjects);
        }
        public IMySqlQueryable<T> OnAxis(byte axisNumber, Expression<Func<T, IEnumerable<object>>> axisObjects)
        {
            if (axisNumber > 127)
                throw new PercolatorException("Axis max is 128");
            return OnAxis(axisNumber, false, axisObjects);
        }
        public IMySqlQueryable<T> OnAxis(byte axisNumber, bool isNonEmpty, Expression<Func<T, object>> axisObjects)
        {
            if (axisNumber > 127)
                throw new PercolatorException("Axis max is 128");


            var axis = new Axis<T>(axisNumber, isNonEmpty, axisObjects);
            _axisGroups.Add(axis);
            return this;
        }
        public IMySqlQueryable<T> OnAxis(byte axisNumber, bool isNonEmpty, Expression<Func<T, IEnumerable<object>>> axisObjects)
        {
            if (axisNumber > 127)
                throw new PercolatorException("Axis max is 128");
            var axis = new Axis<T>(axisNumber, isNonEmpty, axisObjects);
            _axisGroups.Add(axis);
            return this;
        }
        public IMySqlQueryable<T> Slice(Expression<Func<T, object>> slicers)
        {
            _components.Add(new ScoopComponent(Component.Where, null, slicers));
            return this;
        }
        public IMySqlQueryable<T> Slice(Expression<Func<T, IEnumerable<object>>> slicers)
        {
            _components.Add(new ScoopComponent(Component.Where, null, slicers));
            return this;
        }
    }
}
