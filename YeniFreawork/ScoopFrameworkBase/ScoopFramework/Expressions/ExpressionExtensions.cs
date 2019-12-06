using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ScoopFramework.Expressions
{
    public static class ExpressionExtensions
    {
        public static string MemberWithoutInstance(this LambdaExpression expression)
        {
            return System.Web.Mvc.ExpressionHelper.GetExpressionText(expression);
        }

        public static bool IsBindable(this LambdaExpression expression)
        {
            switch (expression.Body.NodeType)
            {
                case ExpressionType.MemberAccess:
                case ExpressionType.Parameter:
                    return true;
            }

            return false;
        }

        public static MemberExpression ToMemberExpression(this LambdaExpression expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
            {
                UnaryExpression unaryExpression = expression.Body as UnaryExpression;

                if (unaryExpression != null)
                {
                    memberExpression = unaryExpression.Operand as MemberExpression;
                }
            }

            return memberExpression;
        }


    }
}
