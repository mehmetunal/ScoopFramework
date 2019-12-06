using MySql.Data.MySqlClient;
using ScoopFramework.DataTables;
using ScoopFramework.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ScoopFramework.MySql.Interface
{
    public interface IMySqlQueryable<T>
    {
        IMysqlProvider Provider { get; }


        IMySqlQueryable<T> OnAxis(byte axisNumber, Expression<Func<T, object>> axisObjects);
        IMySqlQueryable<T> OnAxis(byte axisNumber, Expression<Func<T, IEnumerable<object>>> axisObjects);
        IMySqlQueryable<T> OnAxis(byte axisNumber, bool isNonEmpty, Expression<Func<T, object>> axisObjects);
        IMySqlQueryable<T> OnAxis(byte axisNumber, bool isNonEmpty, Expression<Func<T, IEnumerable<object>>> axisObjects);
        IMySqlQueryable<T> Slice(Expression<Func<T, object>> slicers);
        IMySqlQueryable<T> Slice(Expression<Func<T, IEnumerable<object>>> slicers);

        IMySqlQueryable<T> OrderBy(Expression<Func<T, object>> solid);
        IMySqlQueryable<T> OrderByDesc(Expression<Func<T, object>> solid);
        IMySqlQueryable<T> Paggin(int page, int pageCount);
        IMySqlQueryable<T> Count();
        IMySqlQueryable<T> Filtre(T entity);
        IMySqlQueryable<T> DataTablesFiltre(IDataTablesRequest dataTablesRequest);
        IList<T> ExecuteReader(string sqlCommand, params MySqlParameter[] parametre);
        int ExecuteScalar(string sqlCommand, params MySqlParameter[] parametre);
        T FirstOrDefault();
        List<T> RunToList();
        int RunCount();
        ReturnValue Insert<TSource>(TSource entity) where TSource : new();
        ReturnValue Update<TSource>(TSource entity, Expression<Func<TSource, object>> slicer);
        ReturnValue Update<TSource>(TSource entity);
        ReturnValue Delete<TSource>(TSource entity);
        ReturnValue Delete<TSource>(TSource entity, Expression<Func<TSource, object>> slicer);
        string TranslateToScoop();
    }
}
