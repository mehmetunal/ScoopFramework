using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using ScoopFramework.Scoop.GridColunm;

namespace ScoopFramework.Scoop
{
    public static class Grid
    {
        public static MvcHtmlString ScoopGridModelFor<TModel>(this HtmlHelper htmlHelper, IEnumerable<TModel> dataSource)
        {
            return new MvcHtmlString(null);
        }

        public static MvcHtmlString Name(this MvcHtmlString exp, string name)
        {
            return new MvcHtmlString("");
        }

        public static MvcHtmlString Colunms<TModel, TProperty>(this MvcHtmlString exp, Expression<Func<TModel, TProperty>> colunms)
        {
            return new MvcHtmlString("");
        }

     
        public static MvcHtmlString ScoopGridFor<TModel>(this HtmlHelper htmlHelper, IEnumerable<TModel> dataSource,
            params Expression<Func<Colunms>>[] expression)
        {
            var expressions = expression.Select(x => x.Body).ToList();
            foreach (var exp in expressions)
            {
                var initExpression = exp as MemberInitExpression;
                if (initExpression != null)
                {
                    var bindings = initExpression.Bindings;
                    foreach (var memberBinding in bindings)
                    {
                        var memberInitExpression = (((MemberAssignment)(memberBinding)).Expression) as MemberInitExpression;
                        if (memberInitExpression != null)
                        {
                            var expProperty = memberInitExpression.Bindings;

                            foreach (var binding in expProperty)
                            {
                                var name = (binding.Member).Name;

                                if (name == "Property")
                                {
                                    GetExpressionMemberBinding<TModel>(binding);
                                    continue;
                                }
                                var constantExpression = ((MemberAssignment)(binding)).Expression as ConstantExpression;
                                if (constantExpression != null)
                                {
                                    var value = constantExpression.Value;
                                }
                            }
                        }
                    }
                }
            }
            return MvcHtmlString.Empty;
        }

        private static void GetExpressionMemberBinding<T1>(MemberBinding binding)
        {
            var unaryExpression = (((MemberAssignment)(binding)).Expression) as UnaryExpression;
            if (unaryExpression == null) return;
            var expression = (unaryExpression.Operand) as Expression<Func<object>>;
            if (expression == null) return;
            var bExpression = expression.Body;
            var unaryExpression1 = bExpression as UnaryExpression;
            if (unaryExpression1 == null) return;
            var memberExpression = unaryExpression1.Operand as MemberExpression;
            if (memberExpression != null)
            {
                var propertyName = memberExpression.Member.Name;
            }
        }

        private static StringBuilder tFother(Bount bount, string data)
        {
            var builder = new StringBuilder();
            var tbody = string.Format("<th {0} {1}  >{2}</th>",
                (!string.IsNullOrEmpty(bount.Class) ? bount.Class : ""),
                (bount.Width != default(int) ? bount.Width.ToString() : ""),
                (!string.IsNullOrEmpty(bount.Title) ? bount.Title : "#")
                );
            return new StringBuilder("");
        }
    }
}