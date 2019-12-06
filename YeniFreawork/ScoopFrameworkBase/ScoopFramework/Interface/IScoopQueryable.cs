using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq.Expressions;
using ScoopFramework.DataTables;
using ScoopFramework.Helper;
using ScoopFramework.Linq;
using ScoopFramework.Model;

namespace ScoopFramework.Interface
{
    public interface IScoopQueryable<T>
    {
        /// <summary>
        /// The current collection of axes waiting to be queried against.
        /// </summary>
        List<Axis<object>> AxisCollection { get; }
        /// <summary>
        /// The current collection of Mdx components (Slicers, Subcubes, etc) that are waiting to be queried against.
        /// </summary>
        List<ScoopComponent> Components { get; }
        /// <summary>
        /// Applies mdx objects to an axis and stores the axis in this object to be queried.
        /// </summary>
        /// <param name="axisObjects">An expression to build the objects that will be present when the axis is queried.</param>
        /// <returns>This IMdxQueryable object.</returns>
        IScoopQueryable<T> OnAxis(byte axisNumber, Expression<Func<T, object>> axisObjects);
        /// <summary>
        /// Applies mdx objects to an axis and stores the axis in this object to be queried.
        /// </summary>
        /// <param name="axisObjects">An expression to build the objects that will be present when the axis is queried.</param>
        /// <returns>This IMdxQueryable object.</returns>
        IScoopQueryable<T> OnAxis(byte axisNumber, Expression<Func<T, IEnumerable<object>>> axisObjects);
        /// <summary>
        /// Applies mdx objects to an axis and stores the axis in this object to be queried.
        /// </summary>
        /// <param name="isNonEmpty">Specifies whether the axis should be queried as "NON EMPTY".</param>
        /// <param name="axisObjects">An expression to build the objects that will be present when the axis is queried</param>
        /// <returns>This IMdxQueryable object.</returns>
        IScoopQueryable<T> OnAxis(byte axisNumber, bool isNonEmpty, Expression<Func<T, object>> axisObjects);
        /// <summary>
        /// Applies mdx objects to an axis and stores the axis in this object to be queried.
        /// </summary>
        /// <param name="isNonEmpty">Specifies whether the axis should be queried as "NON EMPTY".</param>
        /// <param name="axisObjects">An expression to build the objects that will be present when the axis is queried</param>
        /// <returns>This IMdxQueryable object.</returns>
        IScoopQueryable<T> OnAxis(byte axisNumber, bool isNonEmpty, Expression<Func<T, IEnumerable<object>>> axisObjects);
        /// <summary>
        /// Applies the "WHERE" slicer to the Mdx query.
        /// </summary>
        /// <param name="slicers">The expression to build the slicer statement.</param>
        /// <returns></returns>
        IScoopQueryable<T> Slice(Expression<Func<T, object>> slicers);
        /// <summary>
        /// Applies the "WHERE" slicer to the Mdx query.
        /// </summary>
        /// <param name="slicers">The expression to build the slicer statement.</param>
        /// <returns></returns>
        IScoopQueryable<T> Slice(Expression<Func<T, IEnumerable<object>>> slicers);
        /// <summary>
        /// Introduces a new query scoped calculated member and stores it to be queried.
        /// </summary>
        /// <param name="name">The name of the calculated member.</param>
        /// <param name="memberCreator">The expression to create this calculated member.</param>
        /// <returns></returns>
        IScoopQueryable<T> WithMember(string name, byte? axisNumber, Expression<Func<T, Member>> memberCreator);
        /// <summary>
        /// Introduces a new query scoped calculated set and stores it to be queried.
        /// </summary>
        /// <param name="name">The name of the calculated set.</param>
        /// <param name="setCreator">The expression to create this calculated set.</param>
        /// <returns></returns>
        IScoopQueryable<T> WithSet(string name, byte? axisNumber, Expression<Func<T, Set>> setCreator);
        /// <summary>
        /// Introduces a sub cube that will be used in the query.
        /// </summary>
        /// <param name="subCube">The expression to create the sub cube.</param>
        /// <returns></returns>
        IScoopQueryable<T> FromSubCube(Expression<Func<T, object>> subCube);
        IScoopQueryable<T> OrderBy(Expression<Func<T, object>> solid);
        IScoopQueryable<T> OrderByDesc(Expression<Func<T, object>> solid);
        IScoopQueryable<T> Paggin(int page, int pageCount);
        IScoopQueryable<T> Count();

        IScoopQueryable<T> Filtre(T entity);
        /// <summary>
        /// [ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        IScoopQueryable<T> DataTablesFiltre(IDataTablesRequest entity);


        IList<T> ExecuteReader(string sqlCommand, params SqlParameter[] parametre);
        int ExecuteScalar(string sqlCommand, params SqlParameter[] parametre);

        IScoopQueryable<T> Between(Expression<Func<T, object>> solid, object start, object end);
        /// <summary>
        /// Introduces a sub cube that will be used in the query.
        /// </summary>
        /// <param name="subCube">The expression to create the sub cube.</param>
        /// <returns></returns>
        IScoopQueryable<T> FromSubCube(Expression<Func<T, IEnumerable<object>>> subCube);
        IEnumerable<TResult> Percolate<TResult>(bool clearQueryContents = true) where TResult : new();
        //CellSet ExecuteCellSet(bool clearQueryContents = true);
        List<T> RunToList(bool clearQueryContents = true);

        T RunFirstOrDefault(bool clearQueryContents = true);
        int RunCount(bool clearQueryContents = true);


        ReturnValue Insert(T entity, bool clearQueryContents = true);
        ReturnValue BulkInsert(List<T> entity, bool clearQueryContents = true, Expression<Func<T, object>> notUpdateColunm = null, string tableName = null);

        ReturnValue Update(T entity, Expression<Func<T, object>> slicer, bool setNull = false);
        ReturnValue Update(T entity, bool setNull = false);
        ReturnValue BulkUpdate(List<T> entity, Expression<Func<T, object>> where = null, string tableName = null);
        ReturnValue BulkDelete(List<T> entity, Expression<Func<T, object>> where = null, string tableName = null);
        ReturnValue Delete(T entity);
        ReturnValue Delete(T entity, Expression<Func<T, object>> slicer);
        /// <summary>
        /// Clears this object's stored query axes and components.
        /// </summary>
        void Clear();
        /// <summary>
        /// Returns the string of the translated MDX query.
        /// </summary>
        /// <returns></returns>
        string TranslateToScoop();

        bool IsTransactionOpen { get; }

        DbTransaction BeginTransaction();
    }
}
