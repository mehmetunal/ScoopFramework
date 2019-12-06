using System;
using System.Linq.Expressions;

namespace ScoopFramework.Expressions
{
    public static class ParseLinq
    {
        public static void GetPropertyAccesses<T, TResult>(this Expression<Func<T, TResult>> expression)
        {
            new ScoopExpressionVisitor().Visit(expression);
        }
    }
}