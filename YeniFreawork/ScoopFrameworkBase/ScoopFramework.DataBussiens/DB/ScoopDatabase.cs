using ScoopFramework.DataTables;
using ScoopFramework.Helper;
using ScoopFramework.Interface;
using ScoopFramework.Linq;
using ScoopFramework.Mapping;
using ScoopFramework.Model;
using ScoopFramework.SqlOperactions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace ScoopFramework.DataBussiens
{
    public class ScoopDatabase<T> : IDisposable
    {
        public Database<T> _database;
        public string _connectionString;
        public bool IsTransactionOpen { get { return _database.IsTransactionOpen; } }

        public ScoopDatabase(string connectionString, DbTransaction transaction = null)
        {
            _connectionString = connectionString;
            _database = new Database<T>(connectionString, transaction);
        }

        public IScoopQueryable<T> Between(Expression<Func<T, object>> solid, object start, object end)
        {
            return _database.Between(solid, start, end);
        }

        public ReturnValue BulkDelete(List<T> entity, Expression<Func<T, object>> where = null, string tableName = null)
        {
            return _database.BulkDelete(entity, where, tableName);
        }

        public ReturnValue BulkInsert(List<T> entity, bool clearQueryContents = true, Expression<Func<T, object>> notUpdateColunm = null, string tableName = null)
        {
            return _database.BulkInsert(entity, clearQueryContents, notUpdateColunm, tableName);
        }

        public ReturnValue BulkUpdate(List<T> entity, Expression<Func<T, object>> where = null, string tableName = null)
        {
            return _database.BulkUpdate(entity, where, tableName);
        }

        public void Clear()
        {
            _database.Clear();
        }

        public IScoopQueryable<T> Count()
        {
            return _database.Count();
        }

        public IScoopQueryable<T> DataTablesFiltre(IDataTablesRequest entity)
        {
            return _database.DataTablesFiltre(entity);
        }

        public ReturnValue Delete(T entity)
        {
            return _database.Delete(entity);
        }

        public ReturnValue Delete(T entity, Expression<Func<T, object>> slicer)
        {
            return _database.Delete(entity, slicer);
        }

        public void Dispose()
        {
            _database.Dispose();
        }

        public IList<T> ExecuteReader(string sqlCommand, params SqlParameter[] parametre)
        {
            return _database.ExecuteReader(sqlCommand, parametre);
        }

        public int ExecuteScalar(string sqlCommand, params SqlParameter[] parametre)
        {
            return _database.ExecuteScalar(sqlCommand, parametre);
        }

        public IScoopQueryable<T> Filtre(T entity)
        {
            return _database.Filtre(entity);
        }

        public IScoopQueryable<T> FromSubCube(Expression<Func<T, object>> subCube)
        {
            return _database.FromSubCube(subCube);
        }

        public IScoopQueryable<T> FromSubCube(Expression<Func<T, IEnumerable<object>>> subCube)
        {
            return _database.FromSubCube(subCube);
        }

        public ReturnValue Insert(T entity, bool clearQueryContents = true)
        {
            return _database.Insert(entity, clearQueryContents);
        }

        public IScoopQueryable<T> OnAxis(byte axisNumber, Expression<Func<T, object>> axisObjects)
        {
            return _database.OnAxis(axisNumber, axisObjects);
        }

        public IScoopQueryable<T> OnAxis(byte axisNumber, Expression<Func<T, IEnumerable<object>>> axisObjects)
        {
            return _database.OnAxis(axisNumber, axisObjects);
        }

        public IScoopQueryable<T> OnAxis(byte axisNumber, bool isNonEmpty, Expression<Func<T, object>> axisObjects)
        {
            return _database.OnAxis(axisNumber, isNonEmpty, axisObjects);
        }

        public IScoopQueryable<T> OnAxis(byte axisNumber, bool isNonEmpty, Expression<Func<T, IEnumerable<object>>> axisObjects)
        {
            return _database.OnAxis(axisNumber, isNonEmpty, axisObjects);
        }

        public IScoopQueryable<T> OrderBy(Expression<Func<T, object>> solid)
        {
            return _database.OrderBy(solid);
        }

        public IScoopQueryable<T> OrderByDesc(Expression<Func<T, object>> solid)
        {
            return _database.OrderByDesc(solid);
        }

        public IScoopQueryable<T> Paggin(int page, int pageCount)
        {
            return _database.Paggin(page, pageCount);
        }

        public IEnumerable<TResult> Percolate<TResult>(bool clearQueryContents = true) where TResult : new()
        {
            return _database.Percolate<TResult>(clearQueryContents);
        }

        public int RunCount(bool clearQueryContents = true)
        {
            return _database.RunCount(clearQueryContents);
        }

        public T RunFirstOrDefault(bool clearQueryContents = true)
        {
            return _database.RunFirstOrDefault(clearQueryContents);
        }

        public List<T> RunToList(bool clearQueryContents = true)
        {
            return _database.RunToList(clearQueryContents);
        }

        public IScoopQueryable<T> Slice(Expression<Func<T, object>> slicers)
        {
            return _database.Slice(slicers);
        }

        public IScoopQueryable<T> Slice(Expression<Func<T, IEnumerable<object>>> slicers)
        {
            return _database.Slice(slicers);
        }

        public string TranslateToScoop()
        {
            return _database.TranslateToScoop();
        }

        public ReturnValue Update(T entity, Expression<Func<T, object>> slicer, bool setNull = false)
        {
            return _database.Update(entity, slicer, setNull);
        }

        public ReturnValue Update(T entity, bool setNull = false)
        {
            return _database.Update(entity, setNull);
        }

        public IScoopQueryable<T> WithMember(string name, byte? axisNumber, Expression<Func<T, Member>> memberCreator)
        {
            return _database.WithMember(name, axisNumber, memberCreator);
        }

        public IScoopQueryable<T> WithSet(string name, byte? axisNumber, Expression<Func<T, Set>> setCreator)
        {
            return _database.WithSet(name, axisNumber, setCreator);
        }

        public DbTransaction BeginTransection()
        {
            return _database.BeginTransaction();
        }
        
        public IScoopQueryable<T> Table(Expression<Func<T, object>> selector = null)
        {
            return _database.Select(selector);
        }

    }
}