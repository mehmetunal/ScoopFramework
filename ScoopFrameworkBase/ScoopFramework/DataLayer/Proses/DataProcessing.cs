using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using ScoopFramework.Attribute;
using ScoopFramework.DataConnection;
using ScoopFramework.DataLayer.Library;
using ScoopFramework.DataLayer.Proses.DataTable;
using ScoopFramework.DataLayer.Proses.Db;
using ScoopFramework.DataLayer.Proses.Entity;
using ScoopFramework.DataLayer.Proses.Operatorler;
using ScoopFramework.DataLayer.Proses.Properties;
using ScoopFramework.Enum;
using ScoopFramework.LogControl;
using ScoopFramework.Model;

namespace ScoopFramework.DataLayer.Proses
{
    public struct DataProcessing<T> where T
        : new()
    {
        #region Saves
        public static ResultObje Saves(T entity, string dbTablo = null)
        {
            var result = new ResultObje();
            try
            {
                if (string.IsNullOrEmpty(dbTablo))
                {
                    dbTablo = (entity).GetType().BaseType.Name == "Object" ? entity.GetType().Name : (entity).GetType().BaseType.Name;
                }

                using (var connection = DBHelper.Connection())
                {

                    var param = string.Empty;
                    var value = string.Empty;

                    var paramValues = new List<ParamValue>();

                    var property = PropertiesTransactions<T>.DefaulRemoveValueProperty(entity, dbTablo);

                    var props = new List<PropertyInfo>(property);

                    EntityValue<T>.GetValue(entity, props, ref param, ref value, ref paramValues);

                    connection.Open();

                    result.id = DataProcessingInsert.Insert(dbTablo, param, value, connection, paramValues);

                    connection.Close();

                    result.success = true;
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.Mesaj = ex.Message;
                throw new Exception(ex.Message);
            }
            return result;
        }
        public static ResultObje Saves(List<ParamValue> paramValues, string table)
        {
            var result = new ResultObje();
            try
            {
                using (var connection = DBHelper.Connection())
                {
                    var param = paramValues.Aggregate(
                        string.Empty, (current, paramValue) => current + string.Format("{0},", paramValue.Key));
                    var value = paramValues.Aggregate(
                        string.Empty, (current, paramValue) => current + string.Format("@{0},", paramValue.Key));

                    result.id = DataProcessingInsert.Insert(table, param, value, connection, paramValues);
                    connection.Close();
                    result.success = true;

                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.Mesaj = ex.Message;
                throw new Exception(ex.Message);

            }

            return result;
        }
        /// <summary>
        /// Daha İyi Bir Şekilde Yazılmalı
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dbTablo"></param>
        /// <returns></returns>
        public static ResultObje Saves(List<T> entity, object dbTablo = null)
        {
            var result = new ResultObje();
            try
            {
                using (var connection = DBHelper.Connection())
                {
                    var tblName = entity.FirstOrDefault();
                    if (tblName != null && dbTablo == null)
                    {
                        dbTablo = (tblName).GetType().BaseType.Name == "Object" ? tblName.GetType().Name : (tblName).GetType().BaseType.Name;
                    }

                    foreach (var item in entity)
                    {
                        var param = string.Empty;
                        var value = string.Empty;

                        var paramValues = new List<ParamValue>();

                        var property = PropertiesTransactions<T>.DefaulRemoveValueProperty(item, dbTablo);

                        var props = new List<PropertyInfo>(property);

                        EntityValue<T>.GetValue(item, props, ref param, ref value, ref paramValues);

                        connection.Open();

                        result.id = DataProcessingInsert.Insert(dbTablo, param, value, connection, paramValues);

                        connection.Close();
                    }
                    result.success = true;
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.Mesaj = ex.Message;
                throw new Exception(ex.Message);
            }
            return result;
        }
        #endregion

        #region Update
        /// <summary>
        /// The sql update entry.
        /// </summary>
        /// <param name="paramValues">
        /// The param values.
        /// </param>
        /// <param name="table">
        /// The table.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public static bool Update(List<ParamValue> paramValues, string table)
        {
            try
            {
                using (var connection = DBHelper.Connection())
                {
                    var param = paramValues.Aggregate(
                        string.Empty,
                        (current, paramValue) => current + string.Format("{0}=@{1}", paramValue.Key, paramValue.Key));

                    var sql = string.Format("{0} {1} {2} {3} ", SqlQueryEnum.Update, table, SqlQueryEnum.SET, param);

                    sql = paramValues.Where(whr => whr.Where != null).SelectMany(
                        whr => whr.Where)
                        .Aggregate(sql,
                            (current, item) => current + string.Format(" Where {0}={1}", item.Key, item.Value));

                    using (var command = new SqlCommand(sql, connection))
                    {
                        connection.Open();

                        foreach (var item in paramValues)
                        {
                            var prm = string.Format("@{0}", item.Key);
                            command.Parameters.AddWithValue(prm, item.Value);
                        }

                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }
        /// <summary>
        /// The entity management update.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public static ResultObje Update(T entity)
        {
            var type = typeof(T);
            if (type.Name == "Object") { type = entity.GetType(); }

            var result = new ResultObje();

            var pkProperty = type.GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).Length > 0)
                .FirstOrDefault();

            try
            {
                var table = (entity).GetType().BaseType.Name == "Object" ? entity.GetType().Name : (entity).GetType().BaseType.Name;

                string selectSql = string.Format("{0} * {1} {2} {3} {4}= @p1", SqlQueryEnum.SELECT, SqlQueryEnum.FROM,
                    table, SqlQueryEnum.WHERE, pkProperty.Name);

                var adapter = new SqlDataAdapter(selectSql, DBHelper.Connection());
                adapter.SelectCommand.Parameters.AddWithValue("@p1", pkProperty.GetValue(entity, null));

                var dTable = new System.Data.DataTable();

                adapter.Fill(dTable);

                var row = dTable.Rows[0];

                EntityValue<T>.GetValueRow(entity, dTable, type, ref row);

                var cmdBuilder = new SqlCommandBuilder(adapter);
                adapter.UpdateCommand = cmdBuilder.GetUpdateCommand();
                result.success = true;
                try
                {
                    adapter.Update(new[] { row });
                }
                catch (Exception ex)
                {
                    result.success = false;
                    result.Mesaj = ex.Message;
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.Mesaj = ex.Message;
                throw new Exception("Pk Attribute Olmadığı İçin Kayıt Yapılamaz. HATA KODU: " + ex.Message);
            }

            return result;
        }
        #endregion

        #region Select
        /// <summary>
        /// The entity management select.
        /// </summary>
        /// <param name="table">
        /// The table.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public static List<T> Select(string table)
        {
            var dataTable = new System.Data.DataTable();
            try
            {
                using (var connection = DBHelper.Connection())
                {
                    var sql = string.Format("{0} * {1} {2} ", SqlQueryEnum.SELECT, SqlQueryEnum.FROM, table);
                    using (var adapter = new SqlDataAdapter(sql, connection))
                    {
                        connection.Open();

                        adapter.Fill(dataTable);
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            var anonimT = DataTableConvert.DataTableToList<T>(dataTable);
            return anonimT;
        }
        /// <summary>
        /// The entity management select.
        /// </summary>
        /// <param name="sqlCommand">
        /// The sql command.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public static List<T> Select(SqlCommand sqlCommand)
        {
            var dataTable = new System.Data.DataTable();
            try
            {
                using (var connection = DBHelper.Connection())
                {
                    using (var adapter = new SqlDataAdapter(sqlCommand.CommandText, connection))
                    {
                        connection.Open();

                        adapter.Fill(dataTable);
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            var anonimT = DataTableConvert.DataTableToList<T>(dataTable);
            return anonimT;
        }

        /// <summary>
        /// The sql select.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="paramValues">
        /// The param values.
        /// </param>
        /// <param name="table">
        /// The table.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>


        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1508:ClosingCurlyBracketsMustNotBePrecededByBlankLine",
            Justification = "Reviewed. Suppression is OK here.")]
        public static List<T> Select(List<ParamValue> paramValues, string table = null)
        {
            System.Data.DataTable dataTable;
            try
            {
                if (string.IsNullOrEmpty(table))
                {
                    table = (typeof(T)).BaseType.Name == "Object" ? typeof(T).Name : (typeof(T)).BaseType.Name;
                }
                using (var connection = DBHelper.Connection())
                {
                    var whereSucces = paramValues.FirstOrDefault(p => p.Where != null && p.Where.Count != 0);

                    var likeSucces = paramValues.FirstOrDefault(p => p.Like != null && p.Like.Count != 0);

                    var sql = SqlOperatorler.SqlWhere(paramValues, table, whereSucces);

                    var firstOrDefault =
                        paramValues.FirstOrDefault(p => !string.IsNullOrEmpty(p.Between) && p.Where != null && p.Where.Count > 0);

                    sql = SqlOperatorler.SqlAnd(firstOrDefault, sql);

                    sql = SqlOperatorler.SqlBetween(paramValues, sql);

                    sql = SqlOperatorler.SqlOrderByAsc(paramValues, sql);

                    sql = SqlOperatorler.SqlLike(paramValues, likeSucces, whereSucces, sql);

                    sql = SqlOperatorler.SqlOrderByDesc(paramValues, sql);

                    sql = SqlOperatorler.SqlPagging(paramValues, sql);

                    dataTable = DataTableProses.DataTableFill(paramValues, sql, connection);

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            var anonimT = DataTableConvert.DataTableToList<T>(dataTable);
            return anonimT;
        }
        #endregion

        #region Delete
        public static bool Delete(List<ParamValue> paramValues, string table)
        {
            try
            {
                using (var connection = DBHelper.Connection())
                {
                    connection.Open();

                    DataProcessingDelete.Delete(paramValues, table, connection);

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }
        public static ResultObje Delete(string pk, string table = null)
        {
            try
            {
                using (var connection = DBHelper.Connection())
                {

                    var result = MethodHelper.SetValuetoClass<object>(table).FirstOrDefault();
                    //var imageColunm = result.GetType().GetProperty()
                    table = (result).GetType().BaseType.Name == "Object" ? result.GetType().Name : (result).GetType().BaseType.Name;
                    var sql = string.Format("{0} {1} {2} {3} id=@id ", SqlQueryEnum.Delete, SqlQueryEnum.FROM, table, SqlQueryEnum.WHERE);
                    using (var command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@id", pk);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultObje() { Mesaj = ex.Message, success = false };

            }
            return new ResultObje() { Mesaj = "Silme İşlemi Başarılı", success = true };

        }
        public static ResultObje Delete(T entity, string table = null)
        {

            try
            {
                using (var connection = DBHelper.Connection())
                {
                    if (string.IsNullOrEmpty(table))
                    {
                        table = (typeof(T)).BaseType.Name == "Object" ? typeof(T).Name : (typeof(T)).BaseType.Name;
                    }


                    var type = typeof(T);
                    var primaryKey = type.GetProperties()
                      .Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).Length > 0)
                      .FirstOrDefault();

                    table = table != null ? table : entity.GetType().Name;

                    var sql = string.Format("{0} {1} {2} {3} {4}=@{5} ", SqlQueryEnum.Delete, SqlQueryEnum.FROM, table, SqlQueryEnum.WHERE, primaryKey.Name, primaryKey.Name);

                    using (var command = new SqlCommand(sql, connection))
                    {
                        var pkValue =
                            entity.GetType()
                                .GetProperties()
                                .Where(info => info.Name == primaryKey.Name)
                                .Select(info => info.GetValue(entity, null))
                                .FirstOrDefault();
                        connection.Open();
                        var prm = string.Format("@{0}", primaryKey.Name);
                        command.Parameters.AddWithValue(prm, pkValue);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultObje() { Mesaj = ex.Message, success = false };

            }
            return new ResultObje() { Mesaj = "Silme İşlemi Başarılı", success = true };

        }
        #endregion

        #region Count
        public static int Count(List<ParamValue> paramValues, string table)
        {
            System.Data.DataTable dataTable;
            try
            {
                using (var connection = DBHelper.Connection())
                {
                    var whereSucces = paramValues.FirstOrDefault(p => p.Where != null && p.Where.Count != 0);

                    var likeSucces = paramValues.FirstOrDefault(p => p.Like != null && p.Like.Count != 0);

                    var sql = SqlOperatorler.SqlWhereCount<T>(paramValues, table, whereSucces);

                    var firstOrDefault = paramValues.FirstOrDefault(p => !string.IsNullOrEmpty(p.Between) && p.Where != null && p.Where.Count > 0);

                    sql = SqlOperatorler.SqlAnd(firstOrDefault, sql);

                    sql = SqlOperatorler.SqlBetween(paramValues, sql);

                    sql = SqlOperatorler.SqlOrderByAsc(paramValues, sql);

                    sql = SqlOperatorler.SqlLike(paramValues, likeSucces, whereSucces, sql);

                    sql = SqlOperatorler.SqlOrderByDesc(paramValues, sql);

                    dataTable = DataTableProses.DataTableFill(paramValues, sql, connection);

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, true, true);
                throw new Exception(ex.Message);
            }

            return Convert.ToInt32(dataTable.Rows[0]["Deger"]);
        }
        #endregion

        #region ScoopSqlCommand
        public static List<T> ScoopSqlCommand(string sql, SqlParameter[] parameter = null)
        {
            var dataTable = new System.Data.DataTable();
            try
            {
                dataTable = DataTableProses.GetDataTableSqlDataAdapter(sql, parameter, dataTable);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            var anonimT = DataTableConvert.DataTableToList<T>(dataTable);
            return anonimT;
        }

        #endregion

        #region ScoopSqlCommandCount
        public static int ScoopSqlCommandCount(string sql, SqlParameter[] parameter = null)
        {
            var dataTable = new System.Data.DataTable();
            try
            {
                dataTable = DataTableProses.GetDataTableSqlDataAdapter(sql, parameter, dataTable);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            return (int)dataTable.Rows[0][0];
        }
        #endregion

        #region Any
        public static bool Any<T1>(List<ParamValue> paramValues, string table)
        {
            var dataTable = new System.Data.DataTable();
            try
            {
                using (var connection = DBHelper.Connection())
                {
                    var whereSucces = paramValues.FirstOrDefault(p => p.Where.Count != 0);
                    var sql = string.Format("{0} * {1} {2} {3} {4} ", SqlQueryEnum.SELECT, SqlQueryEnum.FROM, table,
                        whereSucces != null ? SqlQueryEnum.WHERE.ToString() : string.Empty,
                        string.Join(SqlQueryEnum.AND.ToString(),
                            paramValues.Select(p => p.Where)
                                .SelectMany(whr => whr)
                                .Select(p => string.Format(" {0}=@{1} ", p.Key, p.Key))));

                    var firstOrDefault =
                        paramValues.FirstOrDefault(p => !string.IsNullOrEmpty(p.Between) && p.Where.Count > 0);
                    if (firstOrDefault != null && firstOrDefault.Between.Any())
                    {
                        sql += " AND " + firstOrDefault.Between;
                    }
                    using (var adapter = new SqlDataAdapter(sql, connection))
                    {
                        connection.Open();
                        foreach (var item in paramValues)
                        {
                            foreach (var row in item.Where)
                            {
                                var prm = string.Format("@{0}", row.Key);
                                adapter.SelectCommand.Parameters.AddWithValue(prm, row.Value);
                            }
                        }
                        adapter.Fill(dataTable);
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            return dataTable.Rows.Count > 0;
        }
        #endregion

        #region Filtre
        /// <summary>
        /// The entity management select filter.
        /// </summary>
        /// <param name="paramValues">
        /// The param values.
        /// </param>
        /// <param name="table">
        /// The table.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public static List<T> Filter<T>(List<ParamValue> paramValues, string table)
            where T : new()
        {
            var dataTable = new System.Data.DataTable();
            try
            {
                using (var connection = DBHelper.Connection())
                {

                    var whereSucces = paramValues.FirstOrDefault(p => p.Where.Count != 0);
                    var sql = string.Format("{0} * {1} {2} {3} {4} ", SqlQueryEnum.SELECT, SqlQueryEnum.FROM, table,
                        whereSucces != null ? SqlQueryEnum.WHERE.ToString() : string.Empty,
                        string.Join(SqlQueryEnum.AND.ToString(),
                            paramValues.Select(p => p.Where)
                                .SelectMany(whr => whr)
                                .Select(p => string.Format(" {0}=@{1} ", p.Key, p.Key))));

                    #region düzenlenmesi gerekiyor

                    var firstOrDefault = paramValues.FirstOrDefault(p => p.Between != string.Empty);
                    if (firstOrDefault != null && firstOrDefault.Where.Count > 0 && firstOrDefault.Between.Any())
                    {
                        sql += " AND " + firstOrDefault.Between;
                    }
                    else if (firstOrDefault != null && firstOrDefault.Between.Any())
                    {
                        sql += " WHERE " + firstOrDefault.Between;
                    }
                    if (firstOrDefault != null && firstOrDefault.OrderByAsc != null &&
                        firstOrDefault.OrderByAsc != string.Empty)
                    {
                        sql += " ORDER BY " + firstOrDefault.OrderByAsc + " ASC ";
                    }

                    var like = paramValues.Select(value => value.Like).FirstOrDefault();
                    if (like != null && like.Any())
                    {
                        sql += " LIKE  '%" + like + "%'";
                    }

                    var orderByDesc = paramValues.Select(value => value.OrderByDescending).ToList();
                    if (orderByDesc.FirstOrDefault() != null && orderByDesc.Any())
                    {
                        var desc = string.Empty;
                        foreach (var item in orderByDesc.FirstOrDefault())
                        {
                            desc += item + " DESC,";
                        }
                        sql += string.Format("ORDER BY {0} ", desc.TrimEnd(','));
                    }

                    #endregion

                    using (var adapter = new SqlDataAdapter(sql.Trim(), connection))
                    {
                        connection.Open();
                        foreach (var item in paramValues)
                        {
                            foreach (var row in item.Where)
                            {
                                var prm = string.Format("@{0}", row.Key);
                                adapter.SelectCommand.Parameters.AddWithValue(prm, row.Value);
                            }
                        }

                        adapter.Fill(dataTable);
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            var anonimT = DataTableConvert.DataTableToList<T>(dataTable);
            return anonimT;
        }
        #endregion

        #region Generict Filtre
        public static string GenericFiltre<TType>(TType entity)
        {

            var sart = string.Empty;
            foreach (var param in entity.GetType().GetProperties())
            {
                var value = param.GetValue(entity, null);

                var defaultValue = GetDefault(param.PropertyType);

                if (string.IsNullOrEmpty(sart) && value != null && !string.IsNullOrEmpty(value.ToString()) && !value.Equals(defaultValue))
                {
                    sart = GetSqlWhere<TType>(param, sart, value, false);
                }
                else if (!string.IsNullOrEmpty(sart) && value != null && !string.IsNullOrEmpty(value.ToString()) && !value.Equals(defaultValue))
                {
                    sart = GetSqlWhere<TType>(param, sart, value, true);
                }
            }
            return sart;
        }

        private static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        private static string GetSqlWhere<TType>(PropertyInfo param, string sart, object value, bool and)
        {
            if (param.PropertyType.FullName == "System.DateTime")
            {
                var startAttribute = param.GetCustomAttributes(typeof(StartDateAttribute), false);
                var colunmParam = RealAttributeGetParamValue(param);
                if (startAttribute.Count() > 0)
                {
                    sart += string.Format(" {0} {1}>='{2:yyyy-MM-dd}' ", and ? " and " : "", !string.IsNullOrEmpty(colunmParam) ? colunmParam : param.Name, value);
                }

                var endAttribute = param.GetCustomAttributes(typeof(EndDateAttribute), false);
                if (endAttribute.Count() > 0)
                {
                    sart += string.Format(" {0} {1} <='{2:yyyy-MM-dd}' ", and ? " and " : "", !string.IsNullOrEmpty(colunmParam) ? colunmParam : param.Name, value);
                }


                if (startAttribute.Count() == 0 && endAttribute.Count() == 0)
                {
                    sart += string.Format(" {0} {1} ='{2:yyyy-MM-dd}' ", and ? " and " : "", param.Name, value);
                }
            }
            else
            {
                sart += string.Format("  {0}  {1}='{2}' ", and ? " and " : "", param.Name, value);
            }
            return sart;
        }

        private static string RealAttributeGetParamValue(System.Reflection.PropertyInfo param)
        {
            var colunmParam = "";
            var attr = param.GetCustomAttributesData().ToList();
            foreach (var customAttributeTypedArgument in attr.Select(customAttributeData =>
                (customAttributeData.ConstructorArguments as
                System.Collections.ObjectModel.ReadOnlyCollection
                    <System.Reflection.CustomAttributeTypedArgument>)).SelectMany(asd => asd))
            {
                colunmParam = customAttributeTypedArgument.Value.ToString();
            }
            return colunmParam;
        }

        /// <summary>
        /// İki aynı entityydeki propertylerin hangisinin verisi değiştiğini gösterir ve değişmişi veriyi verir;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sessionEntity"></param>
        /// <param name="changingEntity"></param>
        /// <returns></returns>
        private static T GetPropertySessionValueChange<T>(T sessionEntity, T changingEntity)
        {
            foreach (var prop in sessionEntity.GetType().GetProperties())
            {
                var prop2 = changingEntity.GetType().GetProperties().FirstOrDefault(info => info.Name.Equals(prop.Name));
                if (prop2 != null && prop2.GetValue(changingEntity, null) != null)
                {
                    var val1 = prop.GetValue(sessionEntity, null);
                    var val2 = prop2.GetValue(changingEntity, null);

                    if ((val1 == null && val2 != null) || (val1 != null && val2 != null) && (!val1.Equals(val2)))
                    {
                        prop.SetValue(sessionEntity, val2, null);
                    }
                }
            }
            return sessionEntity;
        }
        #endregion
    }

    //static class MyClass
    //{
    //    public static IEnumerable<TSource> Where2<TSource>(this IEnumerable<TSource> source,
    //       Func<TSource, bool> predicate)
    //    {
    //        return null;
    //    }
    //}

}
