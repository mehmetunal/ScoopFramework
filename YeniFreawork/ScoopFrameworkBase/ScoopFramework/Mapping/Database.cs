using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using ScoopFramework.DataTables;
using ScoopFramework.Exception;
using ScoopFramework.Expressions;
using ScoopFramework.Helper;
using ScoopFramework.Interface;
using ScoopFramework.Linq;
using ScoopFramework.Log;
using ScoopFramework.Model;

namespace ScoopFramework.Mapping
{
    public class Database<T> : IScoopQueryable<T>, IDisposable
    {
        private readonly List<Axis<object>> _axisGroups;
        private readonly List<ScoopComponent> _components;
        private readonly IScoopProvider _provider;
        private byte _createdDepth;
        public List<Axis<object>> AxisCollection { get; }
        public List<ScoopComponent> Components { get; }

        public bool IsTransactionOpen { get { return _provider.IsTransactionOpen; } }

        public Database(string connectionString = null, DbTransaction transaction = null)
        {
            _provider = new Providerlator(connectionString, transaction); //provider;
            _createdDepth = 0;
            _axisGroups = new List<Axis<object>>();
            _components = new List<ScoopComponent>();
        }
        public IScoopQueryable<T> OnAxis(byte axisNumber, Expression<Func<T, object>> axisObjects)
        {
            if (axisNumber > 127)
                throw new PercolatorException("Axis max is 128");
            return OnAxis(axisNumber, false, axisObjects);
        }
        public IScoopQueryable<T> OnAxis(byte axisNumber, Expression<Func<T, IEnumerable<object>>> axisObjects)
        {
            if (axisNumber > 127)
                throw new PercolatorException("Axis max is 128");
            return OnAxis(axisNumber, false, axisObjects);
        }
        public IScoopQueryable<T> OnAxis(byte axisNumber, bool isNonEmpty, Expression<Func<T, object>> axisObjects)
        {
            if (axisNumber > 127)
                throw new PercolatorException("Axis max is 128");

            var axis = new Axis<object>(axisNumber, isNonEmpty, axisObjects);
            _axisGroups.Add(axis);
            return this;
        }
        public IScoopQueryable<T> OnAxis(byte axisNumber, bool isNonEmpty, Expression<Func<T, IEnumerable<object>>> axisObjects)
        {
            if (axisNumber > 127)
                throw new PercolatorException("Axis max is 128");
            var axis = new Axis<object>(axisNumber, isNonEmpty, axisObjects);
            _axisGroups.Add(axis);
            return this;
        }
        public IScoopQueryable<T> Slice(Expression<Func<T, object>> slicers)
        {
            _components.Add(new ScoopComponent(Component.Where, null, slicers));
            return this;
        }
        public IScoopQueryable<T> Slice(Expression<Func<T, IEnumerable<object>>> slicers)
        {
            _components.Add(new ScoopComponent(Component.Where, null, slicers));
            return this;
        }
        public string TranslateToScoop()
        {
            return new Percolator<dynamic>(_axisGroups, _components).ScoopCommand;
        }
        public IScoopQueryable<T> WithMember(string name, byte? axisNumber, Expression<Func<T, Member>> memberCreator)
        {
            var comp = new ScoopComponent(Component.CreatedMember, name, memberCreator);
            comp.Axis = axisNumber;
            comp.DeclarationOrder = _createdDepth++;
            _components.Add(comp);
            return this;
        }
        public IScoopQueryable<T> WithSet(string name, byte? axisNumber, Expression<Func<T, Set>> setCreator)
        {
            var comp = new ScoopComponent(Component.CreatedSet, name, setCreator) { Axis = axisNumber, DeclarationOrder = _createdDepth++ };
            _components.Add(comp);
            return this;
        }
        public IScoopQueryable<T> FromSubCube(Expression<Func<T, object>> subCube)
        {
            var comp = new ScoopComponent(Component.SubCube) { Creator = subCube };
            _components.Add(comp);
            return this;
        }

        public IScoopQueryable<T> OrderBy(Expression<Func<T, object>> solid)
        {
            var comp = new ScoopComponent(Component.OrderBy) { Creator = solid };
            _components.Add(comp);
            return this;
        }
        public IScoopQueryable<T> OrderByDesc(Expression<Func<T, object>> solid)
        {
            var comp = new ScoopComponent(Component.OrderByDesc) { Creator = solid };
            _components.Add(comp);
            return this;
        }
        public IScoopQueryable<T> Paggin(int page, int pageCount)
        {
            var comp = new ScoopComponent(Component.Paggin) { ScoopPagging = new ScoopPagging() { Page = page, PageCount = pageCount } };
            _components.Add(comp);
            return this;
        }

        public IScoopQueryable<T> Count()
        {
            var comp = new ScoopComponent(Component.Count);
            _components.Add(comp);
            return this;
        }

        public IScoopQueryable<T> Filtre(T entity)
        {
            var comp = new ScoopComponent(Component.Filtre) { Entity = entity };
            _components.Add(comp);
            return this;
        }

        public IScoopQueryable<T> DataTablesFiltre(IDataTablesRequest dataTablesRequest)
        {
            var comp = new ScoopComponent(Component.DataTablesRequest) { DataTablesRequest = dataTablesRequest };
            _components.Add(comp);
            return this;
        }

        public IList<T> ExecuteReader(string sqlCommand, params SqlParameter[] parametre)
        {
            return _provider.GetList<T>(sqlCommand, parametre.ToList());
        }
        public int ExecuteScalar(string sqlCommand, params SqlParameter[] parametre)
        {
            return _provider.ExecuteScalar(sqlCommand, parametre.ToList());
        }

        public IScoopQueryable<T> LikeLeft(Expression<Func<T, object>> solid, string value)
        {
            throw new NotImplementedException();
        }

        public IScoopQueryable<T> LikeRight(Expression<Func<T, object>> solid, string value)
        {
            throw new NotImplementedException();
        }

        public IScoopQueryable<T> LikeIn(Expression<Func<T, object>> solid, string value)
        {
            throw new NotImplementedException();
        }

        //Yapılmadı bakılacak
        public IScoopQueryable<T> Between(Expression<Func<T, object>> solid, object start, object end)
        {
            //@TODO Yapılmadı bakılacak
            throw new NotImplementedException();
        }
        public IScoopQueryable<T> FromSubCube(Expression<Func<T, IEnumerable<object>>> subCube)
        {
            var comp = new ScoopComponent(Component.SubCube);
            comp.Creator = subCube;
            _components.Add(comp);
            return this;
        }
        public IEnumerable<TResult> Percolate<TResult>(bool clearQueryContents = true) where TResult : new()
        {
            var lator = new Percolator<TResult>(_axisGroups, _components);
            var command = lator.ScoopCommand;
            //var cellSet = this._provider.GetCellSet(command);
            var reader = _provider.GetReader(command);
            if (clearQueryContents)
                Clear();

            var mapper = new Mapperlator<TResult>(reader);

            return mapper;
        }
        //public CellSet ExecuteCellSet(bool clearQueryContents = true)
        //{
        //    throw new NotImplementedException();
        //}
        public Member CreateMember(string name, Func<T, Member> memberCreator)
        {
            var mem = Member.Create(memberCreator);
            mem.Tag = name;
            return mem;
        }
        public Helper.Set CreateSet(string name, Func<T, Helper.Set> setCreator)
        {
            var set = Helper.Set.Create(setCreator);
            set.Tag = name;
            return set;
        }
        public List<T> RunToList(bool clearQueryContents = true)
        {
            var lator = new Percolator<T>(_axisGroups, _components);
            var command = lator.ScoopCommand;
            if (clearQueryContents)
                Clear();
            return _provider.GetList<T>(command, lator._parameter);
        }


        public T RunFirstOrDefault(bool clearQueryContents = true)
        {
            var lator = new Percolator<T>(_axisGroups, _components);
            var command = lator.ScoopCommand;
            if (clearQueryContents)
                Clear();
            return _provider.GetFirstOrDefault<T>(command, lator._parameter);
        }

        public int RunCount(bool clearQueryContents = true)
        {
            var lator = new Percolator<T>(_axisGroups, _components);
            var command = lator.ScoopCommand;
            if (clearQueryContents)
                Clear();
            return _provider.ExecuteScalar(command, lator._parameter);
        }

        public ReturnValue Insert(T entity, bool clearQueryContents = true)
        {
            try
            {
                //var baseName = new[] { "Object", "BaseEntity" };
                //Type entityType;
                //if (baseName.Contains(entity.GetType().BaseType.Name))
                //{
                //    entityType = entity.GetType();
                //}
                //else
                //{
                //    entityType = entity.GetType().BaseType;
                //}

                Type entityType = typeof(T);

                var propery = entityType.GetProperties();

                var paramters = GetSqlParameters(entity, propery);

                var commandText =
                    $"INSERT INTO {entityType.Name} ({paramters.paramKey.TrimEnd(',')}) VALUES ({paramters.paramValue.TrimEnd(',')})";

                if (clearQueryContents)
                    Clear();

                DbTransaction transaction = null;
                var opentransaction = _provider.IsTransactionOpen;
                if (!opentransaction)
                    transaction = _provider.BeginTransaction();


                var exeRun = _provider.Execute(commandText, paramters.Parameters);

                if (!opentransaction)
                {
                    if (exeRun != (int)ReturnNoneQuery.EnumExecuteNonQuery.hatali)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }

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
        public ReturnValue Update(T entity, Expression<Func<T, object>> slicer, bool setNull = false)
        {
            try
            {
                //var baseName = new[] { "Object", "BaseEntity" };
                //Type entityType;
                //if (baseName.Contains(entity.GetType().BaseType.Name))
                //{
                //    entityType = entity.GetType();
                //}
                //else
                //{
                //    entityType = entity.GetType().BaseType;
                //}

                //var propery = entityType.GetProperties();

                //var paramters = GetSqlParameters(entity, propery, setNull);

                //var commandText = $"UPDATE {entityType.Name} SET {paramters.updateParameters.TrimEnd(',')} ";


                Type entityType = typeof(T);

                var propery = entityType.GetProperties();

                var paramters = GetSqlParameters(entity, propery, setNull);

                var commandText = $"UPDATE {entityType.Name} SET {paramters.updateParameters.TrimEnd(',')} ";

                var obj = slicer.GetValueWhere<T>();
                commandText += $" WHERE {obj}";
                Clear();

                DbTransaction transaction = null;
                var opentransaction = _provider.IsTransactionOpen;
                if (!opentransaction)
                    transaction = _provider.BeginTransaction();

                var exeRun = _provider.Execute(commandText, paramters.Parameters);

                if (!opentransaction)
                {
                    if (exeRun != (int)ReturnNoneQuery.EnumExecuteNonQuery.hatali)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }

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
        public ReturnValue Update(T entity, bool setNull = false)
        {
            try
            {
                //var baseName = new[] { "Object", "BaseEntity" };
                //Type entityType;
                //if (baseName.Contains(entity.GetType().BaseType.Name))
                //{
                //    entityType = entity.GetType();
                //}
                //else
                //{
                //    entityType = entity.GetType().BaseType;
                //}
                Type entityType = typeof(T);

                var propery = entityType.GetProperties();

                var primaryKey = GetPrimaryKey(entityType.Name);

                var paramters = GetSqlParameters(entity, propery, primaryKey, setNull);

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

                Clear();

                DbTransaction transaction = null;
                var opentransaction = _provider.IsTransactionOpen;
                if (!opentransaction)
                    transaction = _provider.BeginTransaction();


                var exeRun = _provider.Execute(commandText, paramters.Parameters);

                if (!opentransaction)
                {
                    if (exeRun != (int)ReturnNoneQuery.EnumExecuteNonQuery.hatali)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }

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
        private object GetPrimaryKey(string tableName)
        {
            var getPrimaryKey = $"SELECT Col.Column_Name from {_provider.DbName}.INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab,{_provider.DbName}.INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col WHERE Col.Constraint_Name = Tab.Constraint_Name AND Col.Table_Name = Tab.Table_Name AND Constraint_Type = 'PRIMARY KEY' AND Col.Table_Name LIKE '{tableName}' "; var dataTable = _provider.GetDataTable(getPrimaryKey);
            return dataTable.Rows.Count > 0 ? dataTable.Rows[0][0] : null;
        }
        public ReturnValue Delete(T entity, Expression<Func<T, object>> slicer)
        {
            try
            {
                var sqlParameterList = new List<SqlParameter>();

                //var tableName = typeof(TSource).Name;
                var tableName = typeof(T).Name;

                var commandText = $"DELETE FROM  {tableName} ";

                var obj = slicer.GetDynamicWhere<T>(sqlParameterList);

                commandText += $" WHERE {obj}";

                Clear();

                DbTransaction transaction = null;
                var opentransaction = _provider.IsTransactionOpen;
                if (!opentransaction)
                    transaction = _provider.BeginTransaction();


                var exeRun = _provider.Execute(commandText, sqlParameterList);

                if (!opentransaction)
                {
                    if (exeRun != (int)ReturnNoneQuery.EnumExecuteNonQuery.hatali)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }

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
        public ReturnValue Delete(T entity)
        {
            try
            {
                //var baseName = new[] { "Object", "BaseEntity" };
                //Type entityType;
                //if (baseName.Contains(entity.GetType().BaseType.Name))
                //{
                //    entityType = entity.GetType();
                //}
                //else
                //{
                //    entityType = entity.GetType().BaseType;
                //}
                Type entityType = typeof(T);

                var primaryKey = GetPrimaryKey(entityType.Name);

                var listeParam = new List<SqlParameter>();

                var commandText = $"DELETE FROM  {entityType.Name} ";

                #region PrimaryKey
                if (primaryKey != null)
                {
                    var value = entityType.GetProperty(primaryKey.ToString()).GetValue(entity, null);

                    var prmName = "@" + primaryKey;
                    commandText += $" WHERE {primaryKey}={prmName}";

                    listeParam.Add(new SqlParameter() { ParameterName = prmName, Value = value });
                }
                #endregion

                Clear();


                DbTransaction transaction = null;
                var opentransaction = _provider.IsTransactionOpen;
                if (!opentransaction)
                    transaction = _provider.BeginTransaction();


                var exeRun = _provider.Execute(commandText, listeParam);

                if (!opentransaction)
                {
                    if (exeRun != (int)ReturnNoneQuery.EnumExecuteNonQuery.hatali)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }

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
        public ReturnValue BulkInsert(List<T> entity, bool clearQueryContents = true, Expression<Func<T, object>> exceprexp = null, string tableName = null)
        {
            try
            {

                PropertyInfo[] baseProperties = new PropertyInfo[0];

                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = typeof(T).Name;
                }

                baseProperties = typeof(T).GetProperties();

                //tableName = string.IsNullOrEmpty(tableName)
                //    ? !new[] { "Object", "BaseEntity" }.Contains(typeof(TSource).Name) ? typeof(TSource).Name : typeof(TSource).BaseType.Name
                //    : tableName;


                //if (string.IsNullOrEmpty(tableName))
                //{
                //    if (new[] { "Object", "BaseEntity" }.Contains(typeof(TSource).BaseType.Name) == true)
                //    {
                //        tableName = typeof(TSource).Name;
                //        baseProperties = typeof(TSource).GetProperties();
                //    }
                //    else
                //    {
                //        tableName = typeof(TSource).BaseType.Name;
                //        baseProperties = typeof(TSource).BaseType.GetProperties();
                //    }
                //}





                var except = ExpressionHelper.GetPropertyNames<T, object>(exceprexp).ToArray();
                var dataTable = new DataTable();
                foreach (var prm in entity)
                {
                    var row = dataTable.NewRow();

                    var list = prm.GetType().GetProperties();
                    // var list1 = list.Where(p => p.GetValue(prm, null) != null);

                    //var list2 = list1.Where(p => p.GetValue(prm, null) != null && !string.IsNullOrEmpty(p.GetValue(prm, null).ToString().Trim()));

                    //var param = list2.Where(p => !except.Contains(p.Name));

                    foreach (var p in list)
                    {

                        if (p.SetMethod == null)
                        {
                            continue;
                        }

                        var _value = p.GetValue(prm, null);
                        if (_value == null || string.IsNullOrEmpty(_value.ToString().Trim()) || except.Contains(p.Name))
                        {
                            continue;
                        }

                        if (baseProperties.Any(x => x.Name == p.Name) == false)
                        {
                            continue;
                        }

                        if (!dataTable.Columns.Contains(p.Name))
                        {
                            var column = DatColumn(p.GetValue(prm, null).GetType().Name, p.Name);
                            dataTable.Columns.Add(column);
                        }
                        row[p.Name] = p.GetValue(prm, null);
                    }
                    dataTable.Rows.Add(row);
                }


                DbTransaction transaction = null;
                var opentransaction = _provider.IsTransactionOpen;
                if (!opentransaction)
                    transaction = _provider.BeginTransaction();


                var exeRun = _provider.BulkExecute(tableName, dataTable);

                if (!opentransaction)
                {
                    if (exeRun != (int)ReturnNoneQuery.EnumExecuteNonQuery.hatali)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
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
                new Logger().LogProvider.Error(ex.Message, "Bulk Insert Exception");
                return new ReturnValue() { Id = null, Mesaj = ex.Message, Success = false };
            }
        }

        /// <summary>
        /// Kullanıma Hazır
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="entity"></param>
        /// <param name="clearQueryContents"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public ReturnValue BulkUpdate(List<T> entity, Expression<Func<T, object>> where = null, string tableName = null)
        {
            try
            {
                var dt = entity.ToConvertToDataTable();


                //if (string.IsNullOrEmpty(tableName))
                //{
                //    if (new[] { "Object", "BaseEntity" }.Contains(typeof(TSource).BaseType.Name) == true)
                //    {
                //        tableName = typeof(TSource).Name;
                //    }
                //    else
                //    {
                //        tableName = typeof(TSource).BaseType.Name;
                //    }
                //}


                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = typeof(T).Name;
                }

                var primaryKey = GetPrimaryKey(tableName);

                var createdTable = _provider.CreatedTable(tableName);

                var tempTableName = $"Temp{tableName}";

                var bulkExecute = _provider.BulkExecute(tempTableName, dt);

                var propery = entity.Count() == 0 ? entity.GetType().GetProperties() : entity[0].GetType().GetProperties();

                if (primaryKey != null)
                {
                    propery = propery.Where(x => x.Name != primaryKey.ToString()).ToArray();
                }

                var paramters = propery.Select(x => x.Name).ToList().Aggregate("", (current, param) => current + String.Format("T.[{0}]=Temp.[{0}],", param));

                var commandText = $"UPDATE T  SET {paramters.TrimEnd(',')} FROM {tableName} T INNER JOIN {tempTableName} Temp ON T.{primaryKey}=Temp.{primaryKey} ; DROP TABLE  {tempTableName}";


                DbTransaction transaction = null;
                var opentransaction = _provider.IsTransactionOpen;
                if (!opentransaction)
                    transaction = _provider.BeginTransaction();


                var exeRun = _provider.Execute(commandText, null);

                if (!opentransaction)
                {
                    if (exeRun != (int)ReturnNoneQuery.EnumExecuteNonQuery.hatali)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                Clear();

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
                new Logger().LogProvider.Error(ex.Message, "Bulk Update Exception");
                return new ReturnValue() { Id = null, Mesaj = ex.Message, Success = false };
            }

        }
        /// <summary>
        /// //@where dolu değil ise entity içindeki primarykey göre silme işlemi yapılacaktır.
            //@where dolu ise where şartı @where göre oluşturulmalıdır.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public ReturnValue BulkDelete(List<T> entity, Expression<Func<T, object>> @where = null, string tableName = null)
        {
            try
            {
                string commandText;

                //tableName = string.IsNullOrEmpty(tableName)
                //    ? !new[] { "Object", "BaseEntity" }.Contains(typeof(TSource).Name) ? typeof(TSource).Name : typeof(TSource).BaseType.Name
                //    : tableName;


                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = typeof(T).Name;
                }

                if (@where == null)
                {
                    var primaryKey = GetPrimaryKey(tableName);
                    commandText = GetBulkDeleteQuery(entity, tableName, primaryKey);
                }
                else
                {
                    var memberExpression = (@where.Body) as MemberExpression;
                    if (memberExpression != null)
                    {
                        var columnName = (memberExpression.Member).Name;
                        commandText = GetBulkDeleteQuery(entity, tableName, columnName);
                    }
                    else
                    {
                        return new ReturnValue() { Mesaj = "@where bu parametrede sorun var", Success = false };

                    }
                }


                DbTransaction transaction = null;
                var opentransaction = _provider.IsTransactionOpen;
                if (!opentransaction)
                    transaction = _provider.BeginTransaction();


                var exeRun = _provider.Execute(commandText, null);

                if (!opentransaction)
                {
                    if (exeRun != (int)ReturnNoneQuery.EnumExecuteNonQuery.hatali)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                Clear();

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
                new Logger().LogProvider.Error(ex.Message, "Bulk DELETE Exception");
                return new ReturnValue() { Id = null, Mesaj = ex.Message, Success = false };
            }
        }

        private static string GetBulkDeleteQuery(List<T> entity, string tableName, object columnName)
        {
            string commandText = "";
            string oldValue = "";

            //tableName = string.IsNullOrEmpty(tableName)
            //? !new[] { "Object", "BaseEntity" }.Contains(typeof(TSource).Name) ? typeof(TSource).Name : typeof(TSource).BaseType.Name
            //    : tableName;

            if (string.IsNullOrEmpty(tableName))
            {
                tableName = typeof(T).Name;
            }

            foreach (var item in entity)
            {
                var properties = item.GetType().GetProperty(columnName.ToString());
                if (properties != null)
                {
                    var value = properties.GetValue(item, null).ToString();
                    if (oldValue != value)
                    {
                        commandText += $" DELETE FROM {tableName} where {columnName}='{value}'" + Environment.NewLine;
                        oldValue = value;
                    }
                }
                else
                {
                    return commandText;
                }
            }
            return commandText;
        }

        private static ReturnParameters GetSqlParameters(T entity, PropertyInfo[] propery, object pk = null, bool setNull = false)
        {
            var paramters = new ReturnParameters();
            foreach (var propertyInfo in propery)
            {
                if (propertyInfo.GetValue(entity, null) == null || pk != null && (string)pk == propertyInfo.Name)
                {
                    continue;
                }

                #region test
                //if ((propertyInfo.PropertyType != typeof(Int32) &&
                //    propertyInfo.PropertyType != typeof(int) &&
                //    propertyInfo.PropertyType != typeof(double) &&
                //    propertyInfo.PropertyType != typeof(Double) &&
                //    propertyInfo.PropertyType != typeof(decimal) &&
                //    propertyInfo.PropertyType != typeof(Decimal) &&
                //    propertyInfo.PropertyType != typeof(float) &&
                //    propertyInfo.PropertyType != typeof(Boolean) &&
                //    propertyInfo.PropertyType != typeof(bool) &&
                //    propertyInfo.PropertyType != typeof(Byte)&&
                //    propertyInfo.PropertyType != typeof(byte) &&
                //    propertyInfo.PropertyType != typeof(SByte) &&
                //    propertyInfo.PropertyType != typeof(sbyte))
                //    && propertyInfo.GetValue(entity, null).Equals(propertyInfo.PropertyType.GetDefault()) || propertyInfo.GetValue(entity, null) == null)
                //{
                //    continue;
                //} 
                #endregion

                if (((
                   propertyInfo.PropertyType == typeof(String) ||
                   propertyInfo.PropertyType == typeof(string) ||
                   propertyInfo.PropertyType == typeof(DateTime) ||
                   propertyInfo.PropertyType == typeof(Guid)) &&
                   propertyInfo.GetValue(entity, null).Equals(propertyInfo.PropertyType.GetDefault())))
                {
                    continue;
                }

                if (setNull == false)
                {
                    if (((
                        propertyInfo.PropertyType == typeof(DateTime?) ||
                        propertyInfo.PropertyType == typeof(Guid?)) &&
                        propertyInfo.GetValue(entity, null).Equals(propertyInfo.PropertyType.GetDefault())) ||
                        propertyInfo.GetValue(entity, null) == null
                        )
                    {
                        continue;
                    }
                }



                string prmName;
                if (paramters.Parameters.Any(x => x.ParameterName == "@" + propertyInfo.Name))
                    prmName = "@" + propertyInfo.Name + "1";
                else
                    prmName = "@" + propertyInfo.Name + "";

                paramters.paramKey += "[" + propertyInfo.Name + "],";
                paramters.paramValue += prmName + ",";

                paramters.updateParameters += $"{"[" + propertyInfo.Name + "]"}={prmName},";

                paramters.Parameters.Add(new SqlParameter()
                {
                    ParameterName = prmName,
                    Value = propertyInfo.GetValue(entity, null)
                });
            }
            return paramters;
        }
        public void Clear()
        {
            _axisGroups.Clear();
            _components.Clear();
        }
        public override string ToString()
        {
            return TranslateToScoop();
        }
        private DataColumn DatColumn(string type, string name)
        {
            if (type.Equals(DbType.Guid.ToString()))
                return new DataColumn(name, typeof(Guid));
            if (type.Equals(DbType.DateTime.ToString()))
                return new DataColumn(name, typeof(DateTime));
            if (type.Equals(DbType.Boolean.ToString()))
                return new DataColumn(name, typeof(Boolean));
            if (type.Equals(DbType.Int32.ToString()))
                return new DataColumn(name, typeof(Int32));
            return new DataColumn(name);
        }

        public void Dispose()
        {
            _provider.Dispose();
        }



        public DbTransaction BeginTransaction()
        {
            return _provider.BeginTransaction();
        }

    }
}