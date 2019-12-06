using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ScoopFramework.Expressions
{
    public static class ExpressionBuilder
    {
        public static Expression<Func<TModel, TValue>> Expression<TModel, TValue>(string memberName)
        {
            return (Expression<Func<TModel, TValue>>)Lambda<TModel>(memberName);
        }

        public static LambdaExpression Lambda<T>(string memberName)
        {
            return Lambda<T>(memberName, false);
        }

        public static LambdaExpression Lambda<T>(Type memberType, string memberName, bool checkForNull)
        {
            MemberAccessExpressionBuilderBase expressionBuilder = ExpressionBuilderFactory.MemberAccess(typeof(T), memberType, memberName, checkForNull);

            return expressionBuilder.CreateLambdaExpression();
        }

        public static LambdaExpression Lambda<T>(string memberName, bool checkForNull)
        {
            MemberAccessExpressionBuilderBase expressionBuilder = ExpressionBuilderFactory.MemberAccess(typeof(T), memberName, checkForNull);

            return expressionBuilder.CreateLambdaExpression();
        }
    }
}
