using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ScoopFramework.TTGenerators.Helper
{
    public class EditableObject<TClass> : BaseEditableObject<TClass> where TClass : class,new()
    {
        public EditableObject()
            : this(null, null)
        {

        }
        public EditableObject(TClass original)
            : this(original, null)
        {
        }
        public EditableObject(TClass original, TClass value)
            : base(original ?? new TClass(), value ?? new TClass())
        {
        }
        public void BeginEdit()
        {
            Copy(Value, Original);
        }

        public void CancelEdit()
        {
            Copy(Value, Original);

        }

        public void EndEdit()
        {
            Copy(Original, Value);
        }

    }
    public class BaseEditableObject<TClass> where TClass : class
    {
        public TClass Value { get; private set; }
        public TClass Original { get; private set; }
        public bool IsModifed
        {
            get
            {
                return HasChanges(Value, Original);
            }
        }
        public BaseEditableObject(TClass original, TClass value)
        {
            Original = original;
            Value = value;
            Copy(Value, Original);
        }

        /// <summary>
        /// Destination  Source
        /// </summary>

        public static Action<TClass, TClass> Copy { get; private set; }
        public static Func<TClass, TClass, bool> HasChanges { get; private set; }
        static BaseEditableObject()
        {
            {
                ParameterExpression p = Expression.Parameter(typeof(TClass));
                ParameterExpression p1 = Expression.Parameter(typeof(TClass));
                LabelTarget rt = Expression.Label();
                // (a,b)=> a.name = (nametype)convert(b,nametype);
                var type = typeof(TClass);
                var ex = type.GetProperties().Where(a => a.CanRead && a.CanWrite).Select(a =>
                {
                    return (Expression)Expression.Call(p, a.GetSetMethod(), new[] { Expression.Call(p1, a.GetGetMethod()) });
                }).ToList();

                ex.Add(Expression.Label(rt));

                LambdaExpression l = Expression.Lambda(typeof(Action<TClass, TClass>), Expression.Block(ex), p, p1);


                Copy = (Action<TClass, TClass>)l.Compile();
            }
            {
                ParameterExpression p = Expression.Parameter(typeof(TClass));
                ParameterExpression p1 = Expression.Parameter(typeof(TClass));
                LabelTarget rt = Expression.Label();
                // (a,b)=> a.name = (nametype)convert(b,nametype);
                var type = typeof(TClass);

                var ex = Expression.Not(type.GetProperties().Where(a => a.CanRead && a.CanWrite)
                    .Aggregate<PropertyInfo, Expression>(Expression.Constant(true), (e, a) =>
                        Expression.AndAlso(e,
                            Expression.Equal(
                                Expression.Call(p, a.GetGetMethod()),
                                Expression.Call(p1, a.GetGetMethod())
                            )
                         )
                ));

                //  ex.Add(Expression.Label(rt));

                LambdaExpression l = Expression.Lambda(typeof(Func<TClass, TClass, bool>), Expression.Block(ex), p, p1);
                HasChanges = (Func<TClass, TClass, bool>)l.Compile();
            }
        }



    }
}
