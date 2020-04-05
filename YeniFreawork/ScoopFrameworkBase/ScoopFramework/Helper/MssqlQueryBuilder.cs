using System.Collections.Generic;
using System.Linq;
using ScoopFramework.Interface;

namespace ScoopFramework.Helper
{
    public class MssqlQueryBuilder
    {
        public List<QueryParameter> _parameters;
        ITypeMapper _typeMapper;
        public MssqlQueryBuilder(ITypeMapper typeMapper)
        {
            _parameters = new List<QueryParameter>();
            _typeMapper = typeMapper;
        }
        public string ProcessBEXP(BEXP exp)
        {
            if (exp == null) return "";
            var op = exp.Operator;
            var operand1 = ProcessQueryItem(exp.Operand1);
            var operand2 = ProcessQueryItem(exp.Operand2);
            switch (op)
            {
                case BinaryOperator.And: return string.Format("({0} AND {1})", operand1, operand2);
                case BinaryOperator.Or: return string.Format("({0} OR {1})", operand1, operand2);
                case BinaryOperator.Not: return string.Format("(not ({0}))", operand1);
                case BinaryOperator.Equal: return string.Format("({0} = {1})", operand1, operand2);
                case BinaryOperator.NotEqual: return string.Format("({0} != {1})", operand1, operand2);
                case BinaryOperator.LessThan: return string.Format("({0} < {1})", operand1, operand2);
                case BinaryOperator.GreaterThan: return string.Format("({0} > {1})", operand1, operand2);
                case BinaryOperator.LessThanOrEqual: return string.Format("({0} <= {1})", operand1, operand2);
                case BinaryOperator.GreaterThanOrEqual: return string.Format("({0} >= {1})", operand1, operand2);
                case BinaryOperator.Like: return string.Format("({0} LIKE {1})", operand1, operand2);
                case BinaryOperator.In: return (!string.IsNullOrEmpty(operand2) && operand2 != "()") ? string.Format("({0} IN {1})", operand1, operand2) : "1 = 0";
                case BinaryOperator.IsNull: return string.Format("({0} IS NULL)", operand1);
                case BinaryOperator.IsNotNull: return string.Format("({0} IS NOT NULL)", operand1);

                default: throw new QueryBuildException(QueryBuildException.ExceptionTypes.OperatorNotFound);
            }
        }

        private string ProcessQueryItem(IQueryItem item)
        {
            if (item == null) return "";
            else if (item is BEXP) return ProcessBEXP(item as BEXP);
            else if (item is TEXP) return ProcessTEXP(item as TEXP);
            else if (item is FEXP) return ProcessFEXP(item as FEXP);
            else if (item is NEXP) return ProcessNEXP(item as NEXP);
            else if (item is COL) return ProcessCOL(item as COL);
            else if (item is VAL) return ProcessVAL(item as VAL);
            else if (item is ARR) return ProcessARR(item as ARR);
            else if (item is IQueryOrderItem) return ProcessOrderItem(item as IQueryOrderItem);
            return "";

        }
        private  string ProcessTEXP(TEXP exp)
        {
            var op = exp.Operator;
            var operand1 = ProcessQueryItem(exp.Operand1);
            var operand2 = ProcessQueryItem(exp.Operand2);
            switch (op)
            {
                case TransformOperator.Add: return string.Format("({0} + {1})", operand1, operand2);
                case TransformOperator.Divide: return string.Format("({0} / {1})", operand1, operand2);
                case TransformOperator.Modulo: return string.Format("({0} % {1})", operand1, operand2);
                case TransformOperator.Multiply: return string.Format("({0} * {1})", operand1, operand2);
                case TransformOperator.Negate: return string.Format("(-1 * {0})", operand1);
                case TransformOperator.Power: return string.Format("({0} ^ {1})", operand1, operand2);
                case TransformOperator.Subtract: return string.Format("({0} - {1})", operand1, operand2);
                case TransformOperator.Lambda: throw new QueryBuildException(QueryBuildException.ExceptionTypes.OperatorUnsuported);
                case TransformOperator.Conditional: throw new QueryBuildException(QueryBuildException.ExceptionTypes.OperatorUnsuported);
                case TransformOperator.ExclusiveOr: throw new QueryBuildException(QueryBuildException.ExceptionTypes.OperatorUnsuported);
                case TransformOperator.OnesComplement: throw new QueryBuildException(QueryBuildException.ExceptionTypes.OperatorUnsuported);
                default: throw new QueryBuildException(QueryBuildException.ExceptionTypes.OperatorNotFound);
            }
        }
        private  string ProcessFEXP(FEXP exp)
        {
            //if (exp == null) return "";
            var function = exp.Function;
            var parameters = exp.Parameters != null ? exp.Parameters.Select(a => ProcessQueryItem(a)).ToArray() : new string[0];
            switch (function)
            {
                case QueryFunctions.Ascii: return string.Format("ASCII({0})", parameters[0]);
                case QueryFunctions.Char: return string.Format("CHAR({0})", parameters[0]);
                case QueryFunctions.CharIndex:
                    if (parameters.Length == 2) return string.Format("CHARINDEX({0}, {1}) - 1", parameters[0], parameters[1]);
                    else if (parameters.Length == 3) return string.Format("CHARINDEX({0}, {1}, {2}) - 1", parameters[0], parameters[1], parameters[2]);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.Concat: return string.Format("CONCAT({0})", string.Join(",", parameters));
                case QueryFunctions.Difference: return string.Format("DIFFERENCE({0}, {1})", parameters[0], parameters[1]); ;
                case QueryFunctions.Format:
                    if (parameters.Length == 2) return string.Format("FORMAT({0}, {1})", parameters[0], parameters[1]);
                    else if (parameters.Length == 3) return string.Format("FORMAT({0}, {1}, {2})", parameters[0], parameters[1], parameters[2]);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.Left: return string.Format("LEFT({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.Len: return string.Format("LEN({0})", parameters[0]);
                case QueryFunctions.Lower: return string.Format("LOWER({0})", parameters[0]);
                case QueryFunctions.Ltrim: return string.Format("LTRIM({0})", parameters[0]);
                case QueryFunctions.Nchar: return string.Format("NCHAR({0})", parameters[0]);
                case QueryFunctions.Patindex: return string.Format("PATINDEX({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.Quotename:
                    if (parameters.Length == 1) return string.Format("QUOTENAME({0})", parameters[0]);
                    else if (parameters.Length == 2) return string.Format("QUOTENAME({0}, {1})", parameters[0], parameters[1]);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.Replace: return string.Format("REPLACE({0}, {1}, {2})", parameters[0], parameters[1], parameters[2]);
                case QueryFunctions.Replicate: return string.Format("REPLACE({0}, {1})", parameters[0], parameters[1]); ;
                case QueryFunctions.Reverse: return string.Format("REVERSE({0})", parameters[0]);
                case QueryFunctions.Right: return string.Format("RIGHT({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.Rtrim: return string.Format("RTRIM({0})", parameters[0]);
                case QueryFunctions.Trim: return string.Format("LTRIM(RTRIM({0}))", parameters[0]);
                case QueryFunctions.Soundex: return string.Format("SOUNDEX({0})", parameters[0]);
                case QueryFunctions.Space: return string.Format("SPACE({0})", parameters[0]);
                case QueryFunctions.Str:
                    if (parameters.Length == 1) return string.Format("STR({0})", parameters[0]);
                    else if (parameters.Length == 2) return string.Format("STR({0}, {1})", parameters[0], parameters[1]);
                    else if (parameters.Length == 3) return string.Format("SRT({0}, {1}, {2})", parameters[0], parameters[1], parameters[2]);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.String_Escape: return string.Format("STRING_ESCAPE({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.String_Split: return string.Format("STRING_SPLIT({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.Stuff: return string.Format("STUFF({0}, {1}, {2}, {3})", parameters[0], parameters[1], parameters[2], parameters[3]);
                case QueryFunctions.Substring:
                    if (parameters.Length == 2) return string.Format("SUBSTRING({0}, {1}+1, LEN({0})-{1}+1)", parameters[0], parameters[1]);
                    else if (parameters.Length == 3) return string.Format("SUBSTRING({0}, {1}+1, {2})", parameters[0], parameters[1], parameters[2]);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.Unicode: return string.Format("UNICODE({0})", parameters[0]);
                case QueryFunctions.Upper: return string.Format("UPPER({0})", parameters[0]);

                case QueryFunctions.Abs: return string.Format("ABS({0})", parameters[0]);
                case QueryFunctions.Acos: return string.Format("ACOS({0})", parameters[0]);
                case QueryFunctions.Asin: return string.Format("ASIN({0})", parameters[0]);
                case QueryFunctions.Atan: return string.Format("ATAN({0})", parameters[0]);
                case QueryFunctions.Atn2: return string.Format("ATN2({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.Ceiling: return string.Format("CEILING({0})", parameters[0]);
                case QueryFunctions.Cos: return string.Format("COS({0})", parameters[0]);
                case QueryFunctions.Cot: return string.Format("COT({0})", parameters[0]);
                case QueryFunctions.Degrees: return string.Format("DEGREES({0})", parameters[0]);
                case QueryFunctions.Exp: return string.Format("EXP({0})", parameters[0]);
                case QueryFunctions.Floor: return string.Format("FLOOR({0})", parameters[0]);
                case QueryFunctions.Log:
                    if (parameters.Length == 1) return string.Format("LOG({0})", parameters[0]);
                    else if (parameters.Length == 2) return string.Format("LOG({0}, {1})", parameters[0], parameters[1]);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.PI: return "PI()";
                case QueryFunctions.Power: return string.Format("POWER({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.Radians: return string.Format("RADIANS({0})", parameters[0]);
                case QueryFunctions.Rand:
                    if (parameters.Length == 0) return "RAND()";
                    else if (parameters.Length == 1) return string.Format("RAND({0})", parameters[0]);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.Round:
                    if (parameters.Length == 2) return string.Format("ROUND({0}, {1})", parameters[0], parameters[1]);
                    else if (parameters.Length == 3) return string.Format("ROUND({0}, {1}, {2})", parameters[0], parameters[1], parameters[2]);
                    else if (parameters.Length == 1) return string.Format("ROUND({0}, {1})", parameters[0], 0);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.Sign: return string.Format("SIGN({0})", parameters[0]);
                case QueryFunctions.Sin: return string.Format("SIN({0})", parameters[0]);
                case QueryFunctions.Sqrt: return string.Format("SQRT({0})", parameters[0]);
                case QueryFunctions.Square: return string.Format("SQUARE({0})", parameters[0]);
                case QueryFunctions.Tan: return string.Format("TAN({0})", parameters[0]);

                case QueryFunctions.GetDate: return "GETDATE()";

                case QueryFunctions.Avg: return string.Format("AVG({0})", parameters[0]);
                case QueryFunctions.Max: return string.Format("MAX({0})", parameters[0]);
                case QueryFunctions.Min: return string.Format("MIN({0})", parameters[0]);
                case QueryFunctions.Sum: return string.Format("SUM({0})", parameters[0]);
                case QueryFunctions.Stdev: return string.Format("STDEV({0})", parameters[0]);
                case QueryFunctions.Stdevp: return string.Format("STDEVP({0})", parameters[0]);
                case QueryFunctions.Var: return string.Format("VAR({0})", parameters[0]);
                case QueryFunctions.Varp: return string.Format("VARP({0})", parameters[0]);
                case QueryFunctions.Count: return string.Format("COUNT({0})", parameters[0]);
                case QueryFunctions.Count_Big: return string.Format("COUNT_BIG({0})", parameters[0]);
                case QueryFunctions.Grouping: return string.Format("GROUPING({0})", parameters[0]);
                case QueryFunctions.Grouping_Id: return string.Format("GROUPING_ID({0})", parameters[0]);
                case QueryFunctions.Checksum_Agg: return string.Format("CHECKSUM_AGG({0})", parameters[0]);
                default: throw new QueryBuildException(QueryBuildException.ExceptionTypes.OperatorNotFound);
            }
            throw new QueryBuildException(QueryBuildException.ExceptionTypes.OperatorUnsuported);
        }
        private  string ProcessNEXP(NEXP exp)
        {
            var name = exp.Name;
            var expression = ProcessQueryItem(exp.Expression);
            return string.Format("{0} AS [{1}]", expression, name);
        }
        private  string ProcessCOL(COL col)
        {
            if (col.Name == "*") return col.Name;
            return string.Format("[{0}]", col.Name);
        }
        private  string ProcessVAL(VAL val)
        {
            var parameterName = string.Format("@p{0}", _parameters.Count);
            _parameters.Add(new QueryParameter { Name = parameterName, Value = _typeMapper.ConvertToSql(val.Value) });
            return parameterName;
        }
        private  string ProcessARR(ARR arr)
        {
            var values = arr.Values.Select(a => ProcessQueryItem(a)).ToArray();
            return string.Format("({0})", string.Join(",", values));
        }
        private  string ProcessOrderItem(IQueryOrderItem item)
        {
            var value = ProcessQueryItem(item.Value);
            var result = string.Format("{0} {1}", value, item.Type == QueryOrderType.ASC ? "ASC" : "DESC");
            return result;
        }

    }
}
