using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using MySql.Data.MySqlClient;
using ScoopFramework.Exception;
using ScoopFramework.Expressions;
using ScoopFramework.Interface;

namespace ScoopFramework.Helper
{
    public class ParamStringBuilder
    {
        public ParamStringBuilder()
        {
            _parameters = new List<SqlParameter>();
            _MysqlParameters = new List<MySqlParameter>();
            sb = new StringBuilder();
        }
        public StringBuilder sb { get; set; }
        public List<SqlParameter> _parameters { get; set; }
        public List<MySqlParameter> _MysqlParameters { get; set; }
    }
    internal static class NewHelpers
    {
        private static ITypeMapper typeMapper = (ITypeMapper)new MssqlTypeMapper();
        private static List<object> _parameters = new List<object>();
        private static Dictionary<ExpressionType, BinaryOperator> binaryOperators = new Dictionary<ExpressionType, BinaryOperator>();
        private static Dictionary<ExpressionType, TransformOperator> transformOperators;
        private static Dictionary<string, QueryFunctions> staticFunctions;
        private static Dictionary<string, QueryFunctions> aggregteFunctions;
        private static Dictionary<string, QueryFunctions> instanceFunctions;
        private static Dictionary<NewHelpers.FunctionProperty, QueryFunctions> functionProperties;

        static NewHelpers()
        {
            NewHelpers.binaryOperators.Add(ExpressionType.And, BinaryOperator.And);
            NewHelpers.binaryOperators.Add(ExpressionType.AndAlso, BinaryOperator.And);
            NewHelpers.binaryOperators.Add(ExpressionType.Or, BinaryOperator.Or);
            NewHelpers.binaryOperators.Add(ExpressionType.OrElse, BinaryOperator.Or);
            NewHelpers.binaryOperators.Add(ExpressionType.Equal, BinaryOperator.Equal);
            NewHelpers.binaryOperators.Add(ExpressionType.NotEqual, BinaryOperator.NotEqual);
            NewHelpers.binaryOperators.Add(ExpressionType.GreaterThan, BinaryOperator.GreaterThan);
            NewHelpers.binaryOperators.Add(ExpressionType.GreaterThanOrEqual, BinaryOperator.GreaterThanOrEqual);
            NewHelpers.binaryOperators.Add(ExpressionType.LessThan, BinaryOperator.LessThan);
            NewHelpers.binaryOperators.Add(ExpressionType.LessThanOrEqual, BinaryOperator.LessThanOrEqual);
            NewHelpers.transformOperators = new Dictionary<ExpressionType, TransformOperator>();
            NewHelpers.transformOperators.Add(ExpressionType.Add, TransformOperator.Add);
            NewHelpers.transformOperators.Add(ExpressionType.Subtract, TransformOperator.Subtract);
            NewHelpers.transformOperators.Add(ExpressionType.Multiply, TransformOperator.Multiply);
            NewHelpers.transformOperators.Add(ExpressionType.Divide, TransformOperator.Divide);
            NewHelpers.transformOperators.Add(ExpressionType.Modulo, TransformOperator.Modulo);
            NewHelpers.transformOperators.Add(ExpressionType.Power, TransformOperator.Power);
            NewHelpers.staticFunctions = new Dictionary<string, QueryFunctions>();
            NewHelpers.staticFunctions.Add("abs", QueryFunctions.Abs);
            NewHelpers.staticFunctions.Add("ceiling", QueryFunctions.Ceiling);
            NewHelpers.staticFunctions.Add("floor", QueryFunctions.Floor);
            NewHelpers.staticFunctions.Add("sqrt", QueryFunctions.Sqrt);
            NewHelpers.staticFunctions.Add("sign", QueryFunctions.Sign);
            NewHelpers.staticFunctions.Add("acos", QueryFunctions.Acos);
            NewHelpers.staticFunctions.Add("asin", QueryFunctions.Asin);
            NewHelpers.staticFunctions.Add("atan", QueryFunctions.Atan);
            NewHelpers.staticFunctions.Add("exp", QueryFunctions.Exp);
            NewHelpers.staticFunctions.Add("cos", QueryFunctions.Cos);
            NewHelpers.staticFunctions.Add("sin", QueryFunctions.Sin);
            NewHelpers.staticFunctions.Add("tan", QueryFunctions.Tan);
            NewHelpers.staticFunctions.Add("square", QueryFunctions.Square);
            NewHelpers.staticFunctions.Add("radians", QueryFunctions.Radians);
            NewHelpers.staticFunctions.Add("degrees", QueryFunctions.Degrees);
            NewHelpers.instanceFunctions = new Dictionary<string, QueryFunctions>();
            NewHelpers.instanceFunctions.Add("substring", QueryFunctions.Substring);
            NewHelpers.instanceFunctions.Add("tolower", QueryFunctions.Lower);
            NewHelpers.instanceFunctions.Add("toupper", QueryFunctions.Upper);
            NewHelpers.instanceFunctions.Add("replace", QueryFunctions.Replace);
            NewHelpers.instanceFunctions.Add("trimend", QueryFunctions.Rtrim);
            NewHelpers.instanceFunctions.Add("trimstart", QueryFunctions.Ltrim);
            NewHelpers.instanceFunctions.Add("trim", QueryFunctions.Trim);
            NewHelpers.instanceFunctions.Add("reverse", QueryFunctions.Reverse);
            NewHelpers.aggregteFunctions = new Dictionary<string, QueryFunctions>();
            NewHelpers.aggregteFunctions.Add("count", QueryFunctions.Count);
            NewHelpers.aggregteFunctions.Add("max", QueryFunctions.Max);
            NewHelpers.aggregteFunctions.Add("min", QueryFunctions.Min);
            NewHelpers.functionProperties = new Dictionary<NewHelpers.FunctionProperty, QueryFunctions>();
            NewHelpers.functionProperties.Add(new NewHelpers.FunctionProperty()
            {
                ObjectType = typeof(string),
                FunctionName = "length"
            }, QueryFunctions.Len);
        }

        internal static Expression StripQuotes(this Expression source)
        {
            Expression expression = source;
            while (expression is UnaryExpression)
                expression = ((UnaryExpression)expression).Operand;
            return expression;
        }

        internal static ParameterExpression GetParameter(this Expression node)
        {
            Expression expression = node;
            if (expression is BinaryExpression)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;
                ParameterExpression parameter = binaryExpression.Left.GetParameter();
                if (parameter != null)
                    return parameter;
                return binaryExpression.Right.GetParameter();
            }
            while (expression is MemberExpression || expression is MethodCallExpression)
            {
                if (expression is MemberExpression)
                    expression = ((MemberExpression)expression).Expression;
                else if (expression is MethodCallExpression)
                    expression = ((MethodCallExpression)expression).Object;
            }
            return expression as ParameterExpression;
        }

        internal static byte DetermineAxis(this MemberInfo mem)
        {
            string lower = mem.Name.ToLower();
            if (lower == "oncolumns")
                return 0;
            if (lower == "onrows")
                return 1;
            if (lower == "onpages")
                return 2;
            if (lower == "onchapters")
                return 3;
            if (lower == "onsections")
                return 4;
            throw new ArgumentException(string.Format("The {0} axis is not recognized.", (object)mem.Name));
        }

        internal static StringBuilder AppendLine(this StringBuilder source, string str, params object[] objs)
        {
            return source.AppendLine(string.Format(str, objs));
        }

        internal static object GetValue<T>(this Expression expression)
        {
            if (expression == null)
                return (object)null;
            try
            {
                LambdaExpression lambdaExpression = (LambdaExpression)expression;

                var orderByItem = WriteExpression(lambdaExpression.Body);
                if (orderByItem != null && !string.IsNullOrEmpty(((COL)orderByItem).Name))
                {
                    return ((COL)orderByItem).Name;
                }

                    UnaryExpression body = Expression.Lambda<Func<T, object>>(lambdaExpression.Body, new ParameterExpression[1] { lambdaExpression.Parameters.FirstOrDefault<ParameterExpression>() }).Body as UnaryExpression;
                if (body != null)
                {
                    if (body.Operand is MemberExpression)
                    {
                        MemberExpression operand = body.Operand as MemberExpression;
                        if (operand != null)
                            return (object)operand.Member.Name;
                    }
                }
            }
            catch (System.Exception ex)
            {
                return (object)null;
            }
            return (object)null;
        }

        internal static StringBuilder GetDynamicWhere<T>(this Expression expression, List<SqlParameter> _parameter = null)
        {
            _parameter = _parameter ?? new List<SqlParameter>();
            var sb = new StringBuilder();
            expression.StripQuotes();
            var lambdaExpression = (LambdaExpression)expression;

            var unaryExpression = lambdaExpression.Body as UnaryExpression;

            var iqueryItem = WriteExpression(unaryExpression.Operand);
            _parameters.Add(iqueryItem);
            SqlConvert(iqueryItem, sb, _parameter);

            return sb;
        }
        internal static StringBuilder GetDynamicMySqlWhere<T>(this Expression expression, List<MySqlParameter> _parameter = null)
        {
            _parameter = _parameter ?? new List<MySqlParameter>();
            var sb = new StringBuilder();
            expression.StripQuotes();
            var lambdaExpression = (LambdaExpression)expression;

            var unaryExpression = lambdaExpression.Body as UnaryExpression;

            var iqueryItem = WriteExpression(unaryExpression.Operand);
            _parameters.Add(iqueryItem);
            MySqlConvert(iqueryItem, sb, _parameter);
            return sb;
        }

        internal static StringBuilder GetValueWhere<T>(this Expression expression, List<SqlParameter> _parameter = null)
        {
            _parameter = _parameter ?? new List<SqlParameter>();

            var lambdaExp = (LambdaExpression)expression;
            var exp = Expression.Lambda<Func<T, object>>(lambdaExp.Body, lambdaExp.Parameters.FirstOrDefault());

            var sb = new StringBuilder();
            var unaryExpression = exp.Body as UnaryExpression;

            if (unaryExpression != null)
            {
                var iqueryItem = WriteExpression(unaryExpression.Operand);
                _parameters.Add(iqueryItem);
                SqlConvert(iqueryItem, sb, _parameter);
            }
            else
            {
                var _ex = StripQuotes(exp.Body);
                var param = exp.Parameters[0];
                Expression conversion = Expression.Convert(_ex, typeof(object));
                var lambda = Expression.Lambda<Func<T, object>>(conversion, new[] { param });
                var iqueryItem = WriteExpression(lambda.Body);
                _parameters.Add(iqueryItem);
                SqlConvert(iqueryItem, sb, _parameter);

            }
            return sb;
        }

        internal static StringBuilder GetValueMySqlWhere<T>(this Expression expression, List<MySqlParameter> _parameter = null)
        {
            if (_parameters == null)
            {
                _parameter = new List<MySqlParameter>();
            }

            var lambdaExp = (LambdaExpression)expression;
            var exp = Expression.Lambda<Func<T, object>>(lambdaExp.Body, lambdaExp.Parameters.FirstOrDefault());

            var sb = new StringBuilder();
            var unaryExpression = exp.Body as UnaryExpression;

            if (unaryExpression != null)
            {
                var iqueryItem = WriteExpression(unaryExpression.Operand);
                _parameters.Add(iqueryItem);
                MySqlConvert(iqueryItem, sb, _parameter);
            }
            else
            {
                var _ex = StripQuotes(exp.Body);
                var param = exp.Parameters[0];
                Expression conversion = Expression.Convert(_ex, typeof(object));
                var lambda = Expression.Lambda<Func<T, object>>(conversion, new[] { param });
                var iqueryItem = WriteExpression(lambda.Body);
                _parameters.Add(iqueryItem);
                MySqlConvert(iqueryItem, sb, _parameter);

            }
            return sb;
        }

        internal static ParamStringBuilder GetFiltreWhere(this object entity, List<SqlParameter> _parameters = null)
        {
            var obj = new List<string>();
            _parameters = _parameters ?? new List<SqlParameter>();

            var propertyes = entity.GetType().GetProperties().Where(x => x.GetValue(entity, null) != null).ToArray();
            foreach (var propertyInfo in propertyes)
            {
                var value = propertyInfo.GetValue(entity, null);
                _parameters.Add(new SqlParameter() { ParameterName = propertyInfo.Name, Value = value });
                obj.Add("[" + propertyInfo.Name + "]=@" + propertyInfo.Name);
            }


            var sb = new StringBuilder().Append(string.Join(" and ", obj));
            //SqlConvert(iqueryItem, sb, _parameters);

            return new ParamStringBuilder() { sb = sb, _parameters = _parameters };
        }

        internal static ParamStringBuilder GetFiltreMySqlWhere(this object entity, List<MySqlParameter> _parameters = null)
        {
            var obj = new List<string>();
            _parameters = _parameters ?? new List<MySqlParameter>();

            var propertyes = entity.GetType().GetProperties().Where(x => x.GetValue(entity, null) != null).ToArray();
            foreach (var propertyInfo in propertyes)
            {
                var value = propertyInfo.GetValue(entity, null);
                _parameters.Add(new MySqlParameter() { ParameterName = propertyInfo.Name, Value = value });
                obj.Add("[" + propertyInfo.Name + "]=@" + propertyInfo.Name);
            }


            var sb = new StringBuilder().Append(string.Join(" and ", obj));
            //SqlConvert(iqueryItem, sb, _parameters);

            return new ParamStringBuilder() { sb = sb, _MysqlParameters = _parameters };
        }

        private static void MySqlConvert(IQueryItem iqueryItem, StringBuilder sb, List<MySqlParameter> _parameter)
        {
            var mssqlQueryBuilder = new MysqlQueryBuilder(typeMapper);
            sb.Append(mssqlQueryBuilder.ProcessBEXP(iqueryItem as BEXP));
            _parameter.AddRange(mssqlQueryBuilder._parameters.Select(queryParameter => new MySqlParameter(queryParameter.Name, queryParameter.Value)));
        }


        private static void SqlConvert(IQueryItem iqueryItem, StringBuilder sb, List<SqlParameter> _parameter)
        {
            var mssqlQueryBuilder = new MssqlQueryBuilder(typeMapper);
            sb.Append(mssqlQueryBuilder.ProcessBEXP(iqueryItem as BEXP));
            _parameter.AddRange(mssqlQueryBuilder._parameters.Select(queryParameter => new SqlParameter(queryParameter.Name, queryParameter.Value)));
        }

        private static IQueryItem WriteExpression(Expression e)
        {
            BinaryOperator binaryOperator;
            if (binaryOperators.TryGetValue(e.NodeType, out binaryOperator))
            {
                IQueryItem queryItem1 = WriteExpression((e as BinaryExpression).Left);
                IQueryItem queryItem2 = WriteExpression((e as BinaryExpression).Right);
                if ((object)(queryItem1 as VAL) != null && (queryItem1 as VAL).Value == DBNull.Value)
                {
                    if (binaryOperator == BinaryOperator.Equal)
                        return (IQueryItem)new BEXP() { Operand1 = queryItem2, Operator = BinaryOperator.IsNull };
                    if (binaryOperator == BinaryOperator.NotEqual)
                        return (IQueryItem)new BEXP() { Operand1 = queryItem2, Operator = BinaryOperator.IsNotNull };
                }
                if ((object)(queryItem2 as VAL) != null && (queryItem2 as VAL).Value == DBNull.Value)
                {
                    if (binaryOperator == BinaryOperator.Equal)
                        return (IQueryItem)new BEXP() { Operand1 = queryItem1, Operator = BinaryOperator.IsNull };
                    if (binaryOperator == BinaryOperator.NotEqual)
                        return (IQueryItem)new BEXP() { Operand1 = queryItem1, Operator = BinaryOperator.IsNotNull };
                }
                return (IQueryItem)new BEXP() { Operand1 = queryItem1, Operand2 = queryItem2, Operator = binaryOperator };
            }
            if (e.NodeType == ExpressionType.New)
            {
                object obj = Expression.Lambda(e).Compile().DynamicInvoke();
                return (IQueryItem)new VAL() { Value = obj };
            }
            if (e.NodeType == ExpressionType.Not)
            {
                IQueryItem queryItem = NewHelpers.WriteExpression((e as UnaryExpression).Operand);
                return (IQueryItem)new BEXP() { Operand1 = queryItem, Operator = BinaryOperator.Not };
            }
            TransformOperator transformOperator;
            if (NewHelpers.transformOperators.TryGetValue(e.NodeType, out transformOperator))
            {
                IQueryItem queryItem1 = NewHelpers.WriteExpression((e as BinaryExpression).Left);
                IQueryItem queryItem2 = NewHelpers.WriteExpression((e as BinaryExpression).Right);
                return (IQueryItem)new TEXP() { Operand1 = queryItem1, Operand2 = queryItem2, Operator = transformOperator };
            }
            if (e.NodeType == ExpressionType.Negate)
            {
                IQueryItem queryItem = NewHelpers.WriteExpression((e as BinaryExpression).Left);
                return (IQueryItem)new TEXP() { Operand1 = queryItem, Operator = TransformOperator.Negate };
            }
            if (e is MemberExpression)
            {
                MemberExpression exp = e as MemberExpression;
                KeyValuePair<NewHelpers.FunctionProperty, QueryFunctions>[] array = NewHelpers.functionProperties.Where<KeyValuePair<NewHelpers.FunctionProperty, QueryFunctions>>((Func<KeyValuePair<NewHelpers.FunctionProperty, QueryFunctions>, bool>)(a =>
                {
                    if (a.Key.FunctionName == exp.Member.Name.ToLower())
                        return a.Key.ObjectType.IsAssignableFrom(exp.Expression.Type);
                    return false;
                })).ToArray<KeyValuePair<NewHelpers.FunctionProperty, QueryFunctions>>();
                if ((uint)array.Length > 0U)
                {
                    IQueryItem queryItem = NewHelpers.WriteExpression(exp.Expression);
                    return (IQueryItem)new FEXP() { Function = array[0].Value, Parameters = new IQueryValue[1] { (IQueryValue)queryItem } };
                }
                if (exp.Expression is ParameterExpression || exp.Expression is MemberExpression && (exp.Expression as MemberExpression).Expression is ParameterExpression)
                    return (IQueryItem)new COL(exp.Member.Name);
                object obj = Expression.Lambda((Expression)exp).Compile().DynamicInvoke();
                if (obj == null)
                    return (IQueryItem)new VAL() { Value = (object)DBNull.Value };
                if (!obj.GetType().IsArray)
                    return (IQueryItem)new VAL() { Value = obj };
                return (IQueryItem)new ARR() { Values = (IQueryValue[])((Array)obj).Cast<object>().Select<object, VAL>((Func<object, VAL>)(a => new VAL() { Value = a })).ToArray<VAL>() };
            }
            if (e is UnaryExpression)
                return NewHelpers.WriteExpression((e as UnaryExpression).Operand);
            if (e is ConstantExpression)
                return (IQueryItem)new VAL() { Value = Expression.Lambda(e).Compile().DynamicInvoke() };
            if (!(e is MethodCallExpression))
                return (IQueryItem)null;
            MethodCallExpression methodCallExpression = (MethodCallExpression)e;
            QueryFunctions queryFunctions;
            if (NewHelpers.aggregteFunctions.TryGetValue(methodCallExpression.Method.Name.ToLower(CultureInfo.InvariantCulture), out queryFunctions) && methodCallExpression.Object is ParameterExpression)
            {
                List<IQueryItem> source = new List<IQueryItem>();
                foreach (Expression expression in methodCallExpression.Arguments)
                    source.Add(NewHelpers.WriteExpression((expression as LambdaExpression).Body));
                if (methodCallExpression.Method.Name.ToLower() == "count")
                    source.Add((IQueryItem)COL.ALL);
                FEXP fexp = new FEXP();
                fexp.Function = queryFunctions;
                IQueryValue[] array = source.Where<IQueryItem>((Func<IQueryItem, bool>)(a => a is IQueryValue)).Cast<IQueryValue>().ToArray<IQueryValue>();
                fexp.Parameters = array;
                return (IQueryItem)fexp;
            }
            if (methodCallExpression.Method.Name.ToLower(CultureInfo.InvariantCulture) == "asc")
            {
                List<IQueryItem> queryItemList = new List<IQueryItem>();
                foreach (Expression expression in methodCallExpression.Arguments)
                    queryItemList.Add(NewHelpers.WriteExpression((expression as LambdaExpression).Body));
                return (IQueryItem)new ASC() { Value = (IQueryValue)queryItemList[0] };
            }
            if (methodCallExpression.Method.Name.ToLower(CultureInfo.InvariantCulture) == "desc")
            {
                List<IQueryItem> queryItemList = new List<IQueryItem>();
                foreach (Expression expression in methodCallExpression.Arguments)
                    queryItemList.Add(NewHelpers.WriteExpression((expression as LambdaExpression).Body));
                return (IQueryItem)new DESC() { Value = (IQueryValue)queryItemList[0] };
            }
            if (NewHelpers.staticFunctions.TryGetValue(methodCallExpression.Method.Name.ToLower(CultureInfo.InvariantCulture), out queryFunctions))
            {
                List<IQueryItem> source = new List<IQueryItem>();
                foreach (Expression e1 in methodCallExpression.Arguments)
                    source.Add(NewHelpers.WriteExpression(e1));
                FEXP fexp = new FEXP();
                fexp.Function = queryFunctions;
                IQueryValue[] array = source.Where<IQueryItem>((Func<IQueryItem, bool>)(a => a is IQueryValue)).Cast<IQueryValue>().ToArray<IQueryValue>();
                fexp.Parameters = array;
                return (IQueryItem)fexp;
            }
            if (NewHelpers.instanceFunctions.TryGetValue(methodCallExpression.Method.Name.ToLower(CultureInfo.InvariantCulture), out queryFunctions))
            {
                List<IQueryItem> source = new List<IQueryItem>();
                source.Add(NewHelpers.WriteExpression(methodCallExpression.Object));
                foreach (Expression e1 in methodCallExpression.Arguments)
                    source.Add(NewHelpers.WriteExpression(e1));
                FEXP fexp = new FEXP();
                fexp.Function = queryFunctions;
                IQueryValue[] array = source.Where<IQueryItem>((Func<IQueryItem, bool>)(a => a is IQueryValue)).Cast<IQueryValue>().ToArray<IQueryValue>();
                fexp.Parameters = array;
                return (IQueryItem)fexp;
            }
            if (methodCallExpression.Method.Name.ToLower(CultureInfo.InvariantCulture) == "in")
            {
                IQueryItem queryItem1 = NewHelpers.WriteExpression(methodCallExpression.Arguments[0]);
                IQueryItem queryItem2 = NewHelpers.WriteExpression(methodCallExpression.Arguments[1]);
                return (IQueryItem)new BEXP() { Operand1 = queryItem1, Operand2 = queryItem2, Operator = BinaryOperator.In };
            }
            if (((IEnumerable<string>)new string[3] { "contains", "startswith", "endswith" }).Contains<string>(methodCallExpression.Method.Name.ToLower(CultureInfo.InvariantCulture)))
            {
                string lower = methodCallExpression.Method.Name.ToLower(CultureInfo.InvariantCulture);
                List<IQueryItem> queryItemList = new List<IQueryItem>();
                queryItemList.Add(NewHelpers.WriteExpression(methodCallExpression.Object));
                foreach (Expression e1 in methodCallExpression.Arguments)
                    queryItemList.Add(NewHelpers.WriteExpression(e1));
                VAL val = (VAL)"%";
                IQueryItem queryItem1 = queryItemList[1];
                List<IQueryValue> queryValueList = new List<IQueryValue>();
                if (lower == "contains" || lower == "endswith")
                    queryValueList.Add((IQueryValue)val);
                queryValueList.Add((IQueryValue)queryItem1);
                if (lower == "contains" || lower == "startswith")
                    queryValueList.Add((IQueryValue)val);
                FEXP fexp = new FEXP();
                fexp.Function = QueryFunctions.Concat;
                IQueryValue[] array = queryValueList.ToArray();
                fexp.Parameters = array;
                IQueryItem queryItem2 = (IQueryItem)fexp;
                BEXP bexp = new BEXP();
                bexp.Operator = BinaryOperator.Like;
                IQueryItem queryItem3 = queryItemList[0];
                bexp.Operand1 = queryItem3;
                IQueryItem queryItem4 = queryItem2;
                bexp.Operand2 = queryItem4;
                return (IQueryItem)bexp;
            }
            object obj1 = Expression.Lambda((Expression)methodCallExpression).Compile().DynamicInvoke();
            if (obj1 == null)
                return (IQueryItem)new VAL() { Value = (object)DBNull.Value };
            if (!obj1.GetType().IsArray)
                return (IQueryItem)new VAL() { Value = obj1 };
            return (IQueryItem)new ARR() { Values = (IQueryValue[])((Array)obj1).Cast<object>().Select<object, VAL>((Func<object, VAL>)(a => new VAL() { Value = a })).ToArray<VAL>() };
        }

        public static NewExpression VisitNew(NewExpression nex)
        {
            IEnumerable<Expression> arguments = (IEnumerable<Expression>)NewHelpers.VisitExpressionList(nex.Arguments);
            if (arguments == nex.Arguments)
                return nex;
            if (nex.Members != null)
                return Expression.New(nex.Constructor, arguments, (IEnumerable<MemberInfo>)nex.Members);
            return Expression.New(nex.Constructor, arguments);
        }

        public static ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
        {
            List<Expression> expressionList = (List<Expression>)null;
            int index1 = 0;
            for (int count = original.Count; index1 < count; ++index1)
            {
                Expression expression = new ScoopExpressionVisitor().Visit(original[index1]);
                if (expressionList != null)
                    expressionList.Add(expression);
                else if (expression != original[index1])
                {
                    expressionList = new List<Expression>(count);
                    for (int index2 = 0; index2 < index1; ++index2)
                        expressionList.Add(original[index2]);
                    expressionList.Add(expression);
                }
            }
            if (expressionList != null)
                return expressionList.AsReadOnly();
            return original;
        }

        private static string GetExpresionOperation(BinaryOperator nodeType)
        {
            switch (nodeType)
            {
                case BinaryOperator.Coalesce:
                    return " ?? ";
                case BinaryOperator.Equal:
                    return " = ";
                case BinaryOperator.GreaterThan:
                    return " > ";
                case BinaryOperator.GreaterThanOrEqual:
                    return " >= ";
                case BinaryOperator.LessThan:
                    return " < ";
                case BinaryOperator.LessThanOrEqual:
                    return " <= ";
                case BinaryOperator.NotEqual:
                    return " != ";
                case BinaryOperator.AndAlso:
                    return " AND ";
                case BinaryOperator.OrElse:
                    return " OR ";
                default:
                    return (string)null;
            }
        }

        internal static T GetCubeInstance<T>(this Type source)
        {
            PropertyInfo property = source.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty);
            if (property == (PropertyInfo)null)
                throw new PercolatorException(string.Format("Cannot find the sigleton instance of '{0}'.", (object)typeof(T).Name));
            return (T)property.GetValue((object)null);
        }

        public static object GetDefault(this Type source)
        {
            if (source == (Type)null)
                return (object)null;
            Type type = Nullable.GetUnderlyingType(source);
            if ((object)type == null)
                type = source;
            source = type;
            if (source.IsValueType)
                return Activator.CreateInstance(source);
            if (source == typeof(string))
                return (object)string.Empty;
            return (object)null;
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T obj in source)
                action(obj);
        }

        public static void For<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            int num = 0;
            foreach (T obj in source)
                action(obj, num++);
        }

        public static string Remove(this string source, params string[] removal)
        {
            string str = source;
            foreach (string oldValue in removal)
                str = str.Replace(oldValue, "");
            return str;
        }

        public static Tout To<Tsource, Tout>(this Tsource source, Func<Tsource, Tout> function)
        {
            return function(source);
        }

        public static Tout To<Tsource, T1, Tout>(this Tsource source, T1 in1, Func<Tsource, T1, Tout> function)
        {
            return function(source, in1);
        }

        public static Tout To<Tsource, T1, T2, Tout>(this Tsource source, T1 in1, T2 in2, Func<Tsource, T1, T2, Tout> function)
        {
            return function(source, in1, in2);
        }

        public static Tout To<Tsource, T1, T2, T3, Tout>(this Tsource source, T1 in1, T2 in2, T3 in3, Func<Tsource, T1, T2, T3, Tout> function)
        {
            return function(source, in1, in2, in3);
        }

        public static Tout To<Tsource, T1, T2, T3, T4, Tout>(this Tsource source, T1 in1, T2 in2, T3 in3, T4 in4, Func<Tsource, T1, T2, T3, T4, Tout> function)
        {
            return function(source, in1, in2, in3, in4);
        }

        public static Tout To<Tsource, T1, T2, T3, T4, T5, Tout>(this Tsource source, T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, Func<Tsource, T1, T2, T3, T4, T5, Tout> function)
        {
            return function(source, in1, in2, in3, in4, in5);
        }

        public static Tout To<Tsource, T1, T2, T3, T4, T5, T6, Tout>(this Tsource source, T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, Func<Tsource, T1, T2, T3, T4, T5, T6, Tout> function)
        {
            return function(source, in1, in2, in3, in4, in5, in6);
        }

        public static void Finally<Tsource>(this Tsource source, Action<Tsource> action)
        {
            action(source);
        }

        public static void Finally<Tsource, T1>(this Tsource source, T1 in1, Action<Tsource, T1> action)
        {
            action(source, in1);
        }

        public static void Finally<Tsource, T1, T2>(this Tsource source, T1 in1, T2 in2, Action<Tsource, T1, T2> action)
        {
            action(source, in1, in2);
        }

        public static void Finally<Tsource, T1, T2, T3>(this Tsource source, T1 in1, T2 in2, T3 in3, Action<Tsource, T1, T2, T3> action)
        {
            action(source, in1, in2, in3);
        }

        public static void Finally<Tsource, T1, T2, T3, T4>(this Tsource source, T1 in1, T2 in2, T3 in3, T4 in4, Action<Tsource, T1, T2, T3, T4> action)
        {
            action(source, in1, in2, in3, in4);
        }

        public static void Finally<Tsource, T1, T2, T3, T4, T5, T6>(this Tsource source, T1 in1, T2 in2, T3 in3, T4 in4, T5 in5, T6 in6, Action<Tsource, T1, T2, T3, T4, T5, T6> action)
        {
            action(source, in1, in2, in3, in4, in5, in6);
        }

        private class FunctionProperty
        {
            public Type ObjectType { get; set; }

            public string FunctionName { get; set; }
        }
    }
}
