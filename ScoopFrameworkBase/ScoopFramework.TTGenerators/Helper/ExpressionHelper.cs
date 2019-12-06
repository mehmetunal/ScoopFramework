using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ScoopFramework.TTGenerators.Helper
{
    public class ExpressionHelper
    {
        public static MemberInfo GetProperty<T, R>(Expression<Func<T, R>> exp)
        {
            MemberExpression body = exp.Body as MemberExpression;
            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }
            return body.Member;
        }

        public static IEnumerable<MemberInfo> GetProperties<T, R>(Expression<Func<T, R>> exp)
        {
            if (exp == null)
                return Enumerable.Empty<MemberInfo>();

            if (typeof(MemberExpression).IsAssignableFrom(exp.GetType()))
                return new MemberInfo[] { GetProperty(exp) };

            else if (typeof(MemberExpression).IsAssignableFrom(exp.Body.GetType()))
                return new MemberInfo[] { GetProperty(exp) };

            else if (typeof(NewExpression).IsAssignableFrom(exp.Body.GetType()))
            {
                var newexp = (exp.Body as NewExpression);
                var cols = new string[newexp.Arguments.Count];
                return newexp.Arguments.Select(a => (a as MemberExpression).Member);
            }
            else if (typeof(UnaryExpression).IsAssignableFrom(exp.Body.GetType()))
                return new MemberInfo[] { GetProperty(exp) };

            else
            {
                throw new Exception("Expression çözümlenemedi.");
                //return null;
            }
        }

        public static string GetPropertyName<T, R>(Expression<Func<T, R>> exp)
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
                throw new Exception("Expression çözümlenemedi.");
                //return null;
            }
        }
    }
}
