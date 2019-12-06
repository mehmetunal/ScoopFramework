using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ScoopFramework.TTGenerators.Helper
{
    public static class PropertyAccessGenerator
    {
        static object Cnv(object a, Type t)
        {
            if (a is string)
                return a;
            if (a is DBNull) return null;
            if (a is Int64 || a is Double)
            {
                Type nt = Nullable.GetUnderlyingType(t);
                if (nt != null) t = nt;
                return Convert.ChangeType(a, t);
            }
            return a;
        }
        static Cache<string, Action<object, object>> _setCache = new Cache<string, Action<object, object>>(100);

        static Cache<string, Func<object, object>> _getCache = new Cache<string, Func<object, object>>(100);

        public static Action<object, object> SetDelegate(Type type, string name)
        {
            Action<object, object> fnc;
            var key = type.FullName + name;

            if (!_setCache.TryGet(key, out fnc))
            {
                try
                {
                    PropertyInfo pi = type.GetProperty(name);
                    if (pi == null)
                        return null;
                    Expression<Func<object, Type, object>> convert = (a, t) => Cnv(a, t);

                    ParameterExpression p = Expression.Parameter(typeof(object));
                    ParameterExpression p1 = Expression.Parameter(typeof(object));
                    LabelTarget rt = Expression.Label();

                    LambdaExpression l = Expression.Lambda(typeof(Action<object, object>),
                        Expression.Block(
                           Expression.Assign(
                                Expression.Property(Expression.Convert(p, type), pi),
                                Expression.Convert(
                                    Expression.Invoke(convert, p1, Expression.Constant(pi.PropertyType)), pi.PropertyType
                                )
                           ),
                           Expression.Label(rt)
                        )
                     , p, p1);
                    fnc = (Action<object, object>)l.Compile();
                    _setCache.Add(key, fnc);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message); 
                }
            }
            return fnc;
        }

        public static Func<object, object> GetDelegate(Type type, string name)
        {
            Func<object, object> fnc;
            var key = type.Name + name;
            if (!_getCache.TryGet(key, out fnc))
            {
                PropertyInfo pi = type.GetProperty(name);
                if (pi == null)
                    return null;
                System.Linq.Expressions.ParameterExpression p = System.Linq.Expressions.Expression.Parameter(typeof(object));

                LambdaExpression l = Expression.Lambda(typeof(Func<object, object>),
                    Expression.Convert(
                        Expression.Property(
                            Expression.Convert(p, type), pi
                        ), typeof(object)
                     ), p);
                fnc = (Func<object, object>)l.Compile();
                _getCache.Add(key, fnc);
            }
            return fnc;
        }
    }
}
