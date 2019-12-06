using System;
using System.Linq.Expressions;
using ScoopFramework.Helper;
using ScoopFramework.Interface;
using ScoopFramework.MySql.Interface;

namespace ScoopFramework.SqlOperactions
{
    public static class Scoop
    {
        public static IScoopQueryable<TSource> Select<TSource>(this IScoopQueryable<TSource> source, Expression<Func<TSource, object>> selector = null)
        {
            if (selector == null) return source;

            var bod = selector.Body;
            var memberInit = bod as MemberInitExpression;
            if (memberInit == null) return source;
            var expBlocks = memberInit.Reduce() as BlockExpression;
            if (expBlocks == null) return source;
            foreach (var block in expBlocks.Expressions)
            {
                if (!(block is BinaryExpression)) continue;
                var bin = block as BinaryExpression;

                if (!(bin.Left is MemberExpression)) continue;
                var lambda = Expression.Lambda<Func<TSource, object>>(Expression.Convert(bin.Right, typeof(object)), selector.Parameters[0]);
                source.OnAxis(Convert.ToByte(0), lambda);
            }
            return source;
        }

        public static IScoopQueryable<TSource> Where<TSource>(this IScoopQueryable<TSource> source, Expression<Func<TSource, bool>> slicer)
        {
            var exp = slicer.Body.StripQuotes();
            var param = slicer.Parameters[0];
            Expression conversion = Expression.Convert(exp, typeof(object));
            var lambda = Expression.Lambda<Func<TSource, object>>(conversion, new[] { param });
            source.Slice(lambda);
            return source;
        }


        public static IMySqlQueryable<TSource> Select<TSource>(this IMySqlQueryable<TSource> source, Expression<Func<TSource, object>> selector = null)
        {
            if (selector == null) return source;

            var bod = selector.Body;
            var memberInit = bod as MemberInitExpression;
            if (memberInit == null) return source;
            var expBlocks = memberInit.Reduce() as BlockExpression;
            if (expBlocks == null) return source;
            foreach (var block in expBlocks.Expressions)
            {
                if (!(block is BinaryExpression)) continue;
                var bin = block as BinaryExpression;

                if (!(bin.Left is MemberExpression)) continue;
                var lambda = Expression.Lambda<Func<TSource, object>>(Expression.Convert(bin.Right, typeof(object)), selector.Parameters[0]);
                source.OnAxis(Convert.ToByte(0), lambda);
            }
            return source;
        }

        public static IMySqlQueryable<TSource> Where<TSource>(this IMySqlQueryable<TSource> source, Expression<Func<TSource, bool>> slicer)
        {
            var exp = slicer.Body.StripQuotes();
            var param = slicer.Parameters[0];
            Expression conversion = Expression.Convert(exp, typeof(object));
            var lambda = Expression.Lambda<Func<TSource, object>>(conversion, new[] { param });
            source.Slice(lambda);
            return source;
        }

    }
}