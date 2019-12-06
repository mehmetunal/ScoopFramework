using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using ScoopFramework.Helper;

namespace ScoopFramework.Expressions
{
    public class ExpressionHelper
    {
        private static string GetPropertyName<T, R>(Expression<Func<T, R>> exp)
        {
            MemberExpression body = exp.Body as MemberExpression;
            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }
            return body.Member.Name;
        }

        public static IEnumerable<string> GetPropertyNames<T, R>(Expression<Func<T, R>> exp)
        {
            if (exp == null)
                return Enumerable.Empty<string>();

            if (typeof(MemberExpression).IsAssignableFrom(exp.GetType()))
                return new[] { GetPropertyName(exp) };

            else if (typeof(MemberExpression).IsAssignableFrom(exp.Body.GetType()))
                return new[] { GetPropertyName(exp) };

            else if (typeof(NewExpression).IsAssignableFrom(exp.Body.GetType()))
            {
                var newexp = (exp.Body as NewExpression);
                var cols = new string[newexp.Arguments.Count];
                return newexp.Arguments.Select(a => (a as MemberExpression).Member.Name);
            }
            else if (typeof(UnaryExpression).IsAssignableFrom(exp.Body.GetType()))
                return new[] { GetPropertyName(exp) };

            else
            {
                throw new System.Exception("Expression çözümlenemedi.");
                //return null;
            }
        }
    }

    public static class HelperExp
    {
        public static string Url(Expression<Action<UrlHelper>> _Datasource)
        {
            var routeValues = new RouteValueDictionary();

            var body = _Datasource.Body;
            if (body != null)
            {
                var root = body as MethodCallExpression;

                if (root == null) throw new ArgumentException("Call expression expected.");

                var method = root.Method;

                var parameters = method.GetParameters();
                var arguments = root.Arguments;

                for (var i = 0; i < parameters.Length; ++i)
                {
                    try
                    {
                        routeValues[parameters[i].Name] = Evaluate(arguments[i]);
                    }
                    catch (System.Exception ex)
                    {
                        throw new System.Exception(
                            String.Format(
                                "Failed to evaluate argument #{0} of an mvc action call while creating a url, look at the inner exceptions.",
                                i), ex);
                    }
                }
            }
            return GetRouteURL(routeValues);
        }

        private static Object Evaluate(Expression e)
        {
            if (e is ConstantExpression)
            {
                return (e as ConstantExpression).Value;
            }
            else if (e is NewExpression)
            {
                return (e as NewExpression).Arguments[0];
            }
            else
            {
                return Expression.Lambda(e).Compile().DynamicInvoke();
            }
        }

        private static string GetRouteURL(RouteValueDictionary routeValueDictionary)
        {
            var url = "";

            var area = routeValueDictionary["area"];
            var controller = routeValueDictionary["controllerName"];
            var action = routeValueDictionary["actionName"];

            if (area != null)
                url += $"/{area}";

            if (controller != null)
                url += $"/{controller}";

            if (action != null)
                url += $"/{action}";

            return url;
        }
    }
}