// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="DataAccessLayer.cs">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
using ScoopFramework.DataLayer.Proses;
using ScoopFramework.Enum;
using ScoopFramework.Extension;
using ScoopFramework.LogControl;
using ScoopFramework.Model;

namespace ScoopFramework.DataLayer.OldSource
{
    public static class DataAccessLayer<T> where T
        : new()
    {

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
        public static bool EntityManagementUpdate(List<ParamValue> paramValues, string table)
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

        //TODO:TEST YAPILMALIDIR
        /// <summary>
        /// The sql insert.
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
        public static ResultObje EntityManagementInsert(List<ParamValue> paramValues, string table)
        {
            ResultObje result = new ResultObje();
            try
            {
                using (var connection = DBHelper.Connection())
                {
                    var param = paramValues.Aggregate(
                        string.Empty, (current, paramValue) => current + string.Format("{0},", paramValue.Key));
                    var value = paramValues.Aggregate(
                        string.Empty, (current, paramValue) => current + string.Format("@{0},", paramValue.Key));

                    result.id = Insert(table, param, value, connection, paramValues);
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
        /// The sql insert.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="dbTablo">
        /// The db tablo.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public static ResultObje EntityManagementInsert(T entity, string dbTablo = null)
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

                    var property = DefaulRemoveValueProperty(entity, dbTablo);

                    var props = new List<PropertyInfo>(property);

                    GetValue(entity, props, ref param, ref value, ref paramValues);

                    connection.Open();

                    result.id = Insert(dbTablo, param, value, connection, paramValues);

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

        public static ResultObje EntityManagementInsert(List<T> entity, object dbTablo = null)
        {
            var result = new ResultObje();
            try
            {
                using (var connection = DBHelper.Connection())
                {
                    foreach (var item in entity)
                    {
                        var param = string.Empty;
                        var value = string.Empty;

                        var paramValues = new List<ParamValue>();

                        var property = DefaulRemoveValueProperty(item, dbTablo);

                        var props = new List<PropertyInfo>(property);


                        GetValue(item, props, ref param, ref value, ref paramValues);

                        connection.Open();

                        result.id = Insert(dbTablo, param, value, connection, paramValues);

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

        private static IEnumerable<PropertyInfo> DefaulRemoveValueProperty(T entity, object tablo = null)
        {
            var gercekTabloProp = GercekTabloProp(tablo);

            var prop = new List<PropertyInfo>();
            foreach (var propertyInfo in gercekTabloProp)
            {
                foreach (var p in entity.GetType().GetProperties().Where(info => info.Name.Equals(propertyInfo.Name)))
                {
                    if (propertyInfo.Name == "id" || propertyInfo.Name == "Id") continue;
                    if (p.GetValue(entity, null) == null) continue;
                    if (propertyInfo.PropertyType.Name == "DateTime")
                    {
                        var defaulDate = default(DateTime).ToString("yy-MM-dd");

                        var dateTime = Convert.ToDateTime(propertyInfo.GetValue(entity, null).ToString());

                        var itemValue = string.Format("{0:yy-MM-dd}", dateTime);

                        if (defaulDate != itemValue)
                        {
                            prop.Add(entity.GetType().GetProperties().FirstOrDefault(info => info.Name == propertyInfo.Name));
                        }
                        continue;
                    }
                    prop.Add(entity.GetType().GetProperties().FirstOrDefault(info => info.Name == propertyInfo.Name));
                }

            }
            return prop;
        }

        private static List<PropertyInfo> GercekTabloProp(object tablo)
        {
            /*AppDomaime *.dll'leri yükler*/
            //PreLoadDeployedAssemblies();
            var dbClassFromString = MethodHelper.GetDbClassFromString(tablo.ToString());
            List<PropertyInfo> prop = dbClassFromString.GetProperties().ToList();
            return prop;
        }

        /// <summary>
        /// The get value.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="props">
        /// The props.
        /// </param>
        /// <param name="param">
        /// The param.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="paramValues">
        /// The param values.
        /// </param>
        private static void GetValue(T entity, List<PropertyInfo> props, ref string param, ref string value,
            ref List<ParamValue> paramValues)
        {
            var type = typeof(T);
            var pkProperty = type.GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).Length > 0)
                .FirstOrDefault();
            props.Remove(pkProperty);

            param = string.Join(",", props.Select(p => string.Format("{0}", p.Name)));
            value = string.Join(",", props.Select(p => string.Format("@{0}", p.Name)));

            paramValues.AddRange(
                props.Select(prop => new ParamValue()
                {
                    Key = prop.Name,
                    Value = prop.GetValue(entity, null)
                }));

        }

        /// <summary>
        /// The sql command insert.
        /// </summary>
        /// <param name="dbTablo">
        /// The db tablo.
        /// </param>
        /// <param name="param">
        /// The param.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="paramValues">
        /// The param values.
        /// </param>
        /// <exception cref="Exception">
        /// </exception>
        private static object Insert(object dbTablo, string param, string value, SqlConnection connection,
            List<ParamValue> paramValues)
        {
            var table = dbTablo.GetType().Name != DBDataTypeEnum.String.ToString()
                ? dbTablo.GetType().Name
                : dbTablo.ToString();
            var sql = string.Format("{0} {1} {2} ({3}) {4} ({5}) SELECT SCOPE_IDENTITY()", SqlQueryEnum.INSERT, SqlQueryEnum.INTO, table, param,
                SqlQueryEnum.VALUES, value);

            using (var command = new SqlCommand(sql, connection))
            {
                var transaction = connection.BeginTransaction("ScoopTransaction");
                // Start a local transaction.
                command.Transaction = transaction;

                try
                {
                    var lgParamValue = "";
                    foreach (var item in paramValues)
                    {
                        var prm = string.Format("@{0}", item.Key);
                        command.Parameters.AddWithValue(prm, item.Value ?? string.Empty);

                        lgParamValue += item.Value ?? "";
                    }


                    var geriDonusId = command.ExecuteScalar();

                    // Attempt to commit the transaction.
                    transaction.Commit();

                    var lgSql = string.Format("{0} {1} {2} ({3}) {4} ({5})", SqlQueryEnum.INSERT, SqlQueryEnum.INTO, table, param,
                      SqlQueryEnum.VALUES, lgParamValue);
                    Logger.Log(lgSql, false, true);

                    return geriDonusId;
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        var logMessage = string.Format("Rollback Exception Type: {0} , Message: {1}", ex2.GetType(), ex2.Message);
                        Logger.Log(logMessage, false, false);
                    }


                    // if (ex.Message == string.Format("Invalid object name '{0}'.", table))
                    // {
                    // bool tabloNameSucces = SearchDataBaseTable(connection, ref table);
                    // if (tabloNameSucces)
                    // {
                    // Insert(entity, table, param, value, connection, paramValues);
                    // return;
                    // }
                    // }
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// The search data base table.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="table">
        /// The table.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        private static bool SearchDataBaseTable(SqlConnection connection, ref string table)
        {
            const string searchTable = @"SELECT tables.TABLE_NAME FROM INFORMATION_SCHEMA.TABLES tables";
            var dataTable = new DataTable();
            using (var adapter = new SqlDataAdapter(searchTable, connection))
            {
                try
                {
                    adapter.Fill(dataTable);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row["TABLE_NAME"].ToString().Substring(0, table.Length).Equals(table))
                        {
                            table = row["TABLE_NAME"].ToString();
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return false;
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
        public static List<T> EntityManagementSelect(List<ParamValue> paramValues, string table = null)
        {
            DataTable dataTable;
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

                    var sql = SqlWhere(paramValues, table, whereSucces);

                    var firstOrDefault =
                        paramValues.FirstOrDefault(p => !string.IsNullOrEmpty(p.Between) && p.Where != null && p.Where.Count > 0);

                    sql = SqlAnd(firstOrDefault, sql);

                    sql = SqlBetween(paramValues, sql);

                    sql = SqlOrderByAsc(paramValues, sql);

                    sql = SqlLike(paramValues, likeSucces, whereSucces, sql);

                    sql = SqlOrderByDesc(paramValues, sql);

                    sql = SqlPagging(paramValues, sql);

                    dataTable = DataTableFill(paramValues, sql, connection);

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            var anonimT = DataTableToList<T>(dataTable);
            return anonimT;
        }

        private static string SqlWhere(List<ParamValue> paramValues, string table, ParamValue whereSucces)
        {
            var sql = string.Format("{0} * {1} {2} {3} {4} ", SqlQueryEnum.SELECT, SqlQueryEnum.FROM, table,
                whereSucces != null ? SqlQueryEnum.WHERE.ToString() : string.Empty,
                whereSucces != null
                    ? string.Join(SqlQueryEnum.AND.ToString(),
                        paramValues.Select(p => p.Where)
                            .SelectMany(whr => whr)
                            .Select(p => string.Format(" {0}=@{1} ", p.Key, p.Key.Replace("<", " ").Replace(">", " "))))
                    : "");
            return sql;
        }

        private static DataTable DataTableFill(List<ParamValue> paramValues, string sql, SqlConnection connection)
        {
            var dataTable = new DataTable();

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

        private static string SqlPagging(List<ParamValue> paramValues, string sql)
        {
            var pageing = paramValues.Select(value => value.page).FirstOrDefault();
            if (pageing != null)
            {
                sql += string.Format("  OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", pageing.start, pageing.end);
            }
            return sql;
        }

        private static string SqlOrderByDesc(List<ParamValue> paramValues, string sql)
        {
            var orderByDesc = paramValues.Select(value => value.OrderByDescending).ToList();
            if (orderByDesc.FirstOrDefault() != null && orderByDesc.Any())
            {
                var desc = string.Empty;
                var orDefault = orderByDesc.FirstOrDefault();
                if (orDefault != null)
                    desc += orDefault + " DESC,";
                sql += string.Format("ORDER BY {0} ", desc.TrimEnd(','));
            }
            return sql;
        }

        private static string SqlLike(List<ParamValue> paramValues, ParamValue likeSucces, ParamValue whereSucces, string sql)
        {
            if (likeSucces != null)
            {
                if (whereSucces == null)
                {
                    sql += " Where ";
                }

                sql += " " + string.Join(SqlQueryEnum.AND.ToString(),
                    paramValues.Select(p => p.Like)
                        .SelectMany(lk => lk)
                        .Select(p => string.Format(" {0} LIKE '{1}%' ", p.Key, p.Value)));
            }
            return sql;
        }

        private static string SqlOrderByAsc(List<ParamValue> paramValues, string sql)
        {
            var orderByAsc = paramValues.Select(value => value.OrderByAsc).FirstOrDefault();
            if (orderByAsc != null && orderByAsc.Any())
            {
                sql += " ORDER BY " + orderByAsc + " ASC";
            }
            return sql;
        }

        private static string SqlBetween(List<ParamValue> paramValues, string sql)
        {
            var betWeenList =
                paramValues.FirstOrDefault(p => !string.IsNullOrEmpty(p.Between) && p.Where == null);
            if (betWeenList != null && betWeenList.Between.Any())
            {
                var between = betWeenList.Between.EndsWith("AND")
                    ? betWeenList.Between.Substring(0, betWeenList.Between.Length - 3)
                    : betWeenList.Between;
                sql += " Where " + between;
            }
            return sql;
        }

        private static string SqlAnd(ParamValue firstOrDefault, string sql)
        {
            if (firstOrDefault != null && firstOrDefault.Between.Any())
            {
                var between = firstOrDefault.Between.EndsWith("AND")
                    ? firstOrDefault.Between.Substring(0, firstOrDefault.Between.Length - 3)
                    : firstOrDefault.Between;
                sql += " AND " + between;
            }
            return sql;
        }

        public static int EntityManagementSelectCount<T>(List<ParamValue> paramValues, string table) where T : new()
        {
            DataTable dataTable;
            try
            {
                using (var connection = DBHelper.Connection())
                {
                    var whereSucces = paramValues.FirstOrDefault(p => p.Where != null && p.Where.Count != 0);

                    var likeSucces = paramValues.FirstOrDefault(p => p.Like != null && p.Like.Count != 0);

                    var sql = SqlWhereCount<T>(paramValues, table, whereSucces);

                    var firstOrDefault = paramValues.FirstOrDefault(p => !string.IsNullOrEmpty(p.Between) && p.Where != null && p.Where.Count > 0);

                    sql = SqlAnd(firstOrDefault, sql);

                    sql = SqlBetween(paramValues, sql);

                    sql = SqlOrderByAsc(paramValues, sql);

                    sql = SqlLike(paramValues, likeSucces, whereSucces, sql);

                    sql = SqlOrderByDesc(paramValues, sql);

                    dataTable = DataTableFill(paramValues, sql, connection);

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Convert.ToInt32(dataTable.Rows[0]["Deger"]);
        }

        private static string SqlWhereCount<T>(List<ParamValue> paramValues, string table, ParamValue whereSucces) where T : new()
        {
            var sql = string.Format("{0} Count(*) as Deger {1} {2} {3} {4} ", SqlQueryEnum.SELECT, SqlQueryEnum.FROM, table,
                whereSucces != null ? SqlQueryEnum.WHERE.ToString() : string.Empty,
                whereSucces != null
                    ? string.Join(SqlQueryEnum.AND.ToString(),
                        paramValues.Select(p => p.Where)
                            .SelectMany(whr => whr)
                            .Select(p => string.Format(" {0}=@{1} ", p.Key, p.Key.Replace("<", " ").Replace(">", " "))))
                    : "");
            return sql;
        }

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
        public static List<T> EntityManagementSelect(string table)
        {
            var dataTable = new DataTable();
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

            var anonimT = DataTableToList<T>(dataTable);
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
        public static List<T> EntityManagementSelect(SqlCommand sqlCommand)
        {
            var dataTable = new DataTable();
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

            var anonimT = DataTableToList<T>(dataTable);
            return anonimT;
        }

        /// <summary>
        /// The data table to list.
        /// </summary>
        /// <param name="dataTable">
        /// The data table.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        private static List<T> DataTableToList<T>(DataTable dataTable) where T : new()
        {

            var listItem = new List<T>();
            if (dataTable.Rows.Count > 0)
            {
                var tClass = typeof(T);
                var pClass = tClass.GetProperties();
                var dc = dataTable.Columns.Cast<DataColumn>().ToList();
                foreach (DataRow item in dataTable.Rows)
                {
                    var cn = (T)Activator.CreateInstance(tClass);
                    foreach (var pc in pClass)
                    {
                        try
                        {
                            var d = dc.Find(c => c.ColumnName == pc.Name);
                            if (d != null)
                            {
                                var type00 = item[pc.Name].GetType().Name;
                                //TODO:BURASI DUZELTILECEK;
                                if (type00 == DBDataTypeEnum.Int32.ToString())
                                {
                                    pc.SetValue(cn, item[pc.Name], null);
                                }
                                else if (type00 == DBDataTypeEnum.DBNull.ToString())
                                {
                                    pc.SetValue(cn, (item[pc.Name] == DBNull.Value) ? null : item[pc.Name], null);
                                }
                                else if (type00 == DBDataTypeEnum.DateTime.ToString())
                                {
                                    //var a = String.Format("{0:dd/MM/yyyy}", item[pc.Name]);

                                    var date = Convert.ToDateTime(item[pc.Name], System.Threading.Thread.CurrentThread.CurrentCulture);
                                    //var 
                                    //var col = DateTime.ParseExact(item[pc.Name], format, null);

                                    if (item[pc.Name] == DBNull.Value)
                                    {
                                        pc.SetValue(cn, DateTime.Now, null);
                                    }
                                    else
                                    {
                                        pc.SetValue(cn, date, null);
                                    }

                                }
                                else if (type00 == DBDataTypeEnum.SqlGeography.ToString())
                                {
                                    pc.SetValue(cn, null, null);
                                }
                                else
                                {
                                    pc.SetValue(cn, (item[pc.Name] == DBNull.Value) ? string.Empty : item[pc.Name], null);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message); //"DataTable Liste çevrilemedi.");
                        }
                    }

                    listItem.Add(cn);
                }
            }

            return listItem;
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
        public static bool EntityManagementUpdate(T entity)
        {
            var type = typeof(T);
            if (type.Name == "Object") { type = entity.GetType(); }

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

                var dTable = new DataTable();

                adapter.Fill(dTable);

                var row = dTable.Rows[0];

                GetValueRow(entity, dTable, type, ref row);

                var cmdBuilder = new SqlCommandBuilder(adapter);
                adapter.UpdateCommand = cmdBuilder.GetUpdateCommand();

                try
                {
                    adapter.Update(new[] { row });
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Pk Attribute Olmadığı İçin Kayıt Yapılamaz. HATA KODU: " + ex.Message);
            }

            return true;
        }

        /// <summary>
        /// The get value row.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="dTable">
        /// The d table.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="row">
        /// The row.
        /// </param>
        private static void GetValueRow(T entity, DataTable dTable, Type type, ref DataRow row)
        {
            foreach (var columnName in dTable.Columns.Cast<DataColumn>().Select(cl => cl.ColumnName))
            {
                if (type.GetProperty(columnName).GetValue(entity, null) == null)
                {
                    continue;
                }

                var propertyValue = type.GetProperty(columnName).GetValue(entity, null);

                var property = typeof(T).GetProperty(columnName) ?? entity.GetType().GetProperty(columnName);

                var propertyType = property.PropertyType;

                if (propertyType.Name == "DateTime")
                {
                    var defaulValue = property.PropertyType.GetDefault();

                    if (!row[columnName].Equals(propertyValue) && propertyValue != null && !defaulValue.Equals(propertyValue))
                    {
                        row[columnName] = propertyValue;
                        continue;
                    }
                    var dateValue = dTable.Rows[0][columnName];
                    row[columnName] = dateValue;
                    continue;
                }
                if (!row[columnName].Equals(propertyValue) && propertyValue != null)
                {
                    row[columnName] = propertyValue;
                    continue;
                }

            }

        }

        //private static object GetPropertyValue(string columnName)
        //{
        //    var property = typeof(T).GetProperty(columnName);

        //    Type propertyType = property.PropertyType;
        //    if (propertyType == typeof(String))
        //    {
        //        return string.Empty;
        //    }
        //    return Activator.CreateInstance(propertyType);
        //}

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
        public static List<T> EntityManagementSelectFilter<T>(List<ParamValue> paramValues, string table)
            where T : new()
        {
            var dataTable = new DataTable();
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

            var anonimT = DataTableToList<T>(dataTable);
            return anonimT;
        }

        public static List<T> ScoopSqlCommand(string sql, SqlParameter[] parameter = null)
        {
            var dataTable = new DataTable();
            try
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
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            var anonimT = DataTableToList<T>(dataTable);
            return anonimT;
        }


        public static int ScoopSqlCommandCount(string sql, SqlParameter[] parameter = null)
        {
            var dataTable = new DataTable();
            try
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
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            return (int)dataTable.Rows[0][0];
        }

        public static bool EntityManagementAny<T1>(List<ParamValue> paramValues, string table)
        {
            var dataTable = new DataTable();
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

        /// <summary>
        /// //DELETE FROM TABLO WHERE ID=1
        /// </summary>
        /// <param name="paramValues"></param>
        /// <param name="dbTablo"></param>
        /// <param name="connection"></param>
        private static void Delete(List<ParamValue> paramValues, string dbTablo,
            SqlConnection connection)
        {
            using (var command = new SqlCommand())
            {
                try
                {
                    var table = dbTablo.GetType().Name != DBDataTypeEnum.String.ToString() ? dbTablo.GetType().Name : dbTablo;

                    var sql = string.Format("{0} {1} {2}", SqlQueryEnum.Delete, SqlQueryEnum.FROM, table);

                    sql = paramValues.Where(whr => whr.Where != null).SelectMany(
                                    whr => whr.Where)
                                    .Aggregate(sql,
                                        (current, item) => current + string.Format(" {0} {1}={2}", SqlQueryEnum.WHERE, item.Key, item.Value));

                    command.CommandText = sql;

                    command.Connection = connection;

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        #region Generict Filtre
        public static string GenericFiltre<TType>(TType entity)
        {

            string sart = string.Empty;
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

        public static bool EntityManagementDelete(List<ParamValue> paramValues, string table)
        {
            try
            {
                using (var connection = DBHelper.Connection())
                {
                    connection.Open();

                    Delete(paramValues, table, connection);

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public static ResultObje EntityManagementDelete(string pk, string table = null)
        {
            try
            {
                using (var connection = DBHelper.Connection())
                {
                    var result = MethodHelper.SetValuetoClass<object>(table).FirstOrDefault();

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

        public static ResultObje EntityManagementDelete(T entity, string table = null)
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
    }
}