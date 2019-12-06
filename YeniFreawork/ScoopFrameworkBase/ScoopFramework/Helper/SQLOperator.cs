using System;
using System.Collections.Generic;
using System.Linq;

namespace ScoopFramework.Helper
{

    class SQLOperator
    {
    }
    public enum QueryStatement
    {
        Select,
        Where,
        GroupBy,
        OrderBy,
        Skip,
        Take,
    }


    public interface IQueryItem
    {

    }
    public interface INamedItem : IQueryItem
    {
        string Name { get; set; }
    }
    public interface IQueryValue : IQueryItem
    {
        NEXP this[string val] { get; }
    }
    public interface IExpression
    {

    }
    public interface IQueryOrderItem : IQueryItem
    {
        IQueryValue Value { get; set; }
        QueryOrderType Type { get; }
    }


    public class COL : IQueryItem, IQueryValue, INamedItem
    {
        public string Name { get; set; }
        public object Value { get; set; }
        //public string Param { get; set; }

        public static COL ALL { get { return (COL)"*"; } }

        public COL(string name)
        {
            Name = name;
            //Param = $"@{name}_{Guid.NewGuid()}";
        }
        public COL(string name, object val)
        {
            Name = name;
            Value = val;
            //Param = name;
        }

        public static implicit operator COL(string name)
        {
            return new COL(name);
        }

        public static BEXP operator >(COL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.GreaterThan };
        }
        public static BEXP operator <(COL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.LessThan };
        }
        public static BEXP operator <=(COL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.LessThanOrEqual };
        }
        public static BEXP operator >=(COL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.GreaterThanOrEqual };
        }
        public static BEXP operator ==(COL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.Equal };
        }
        public static BEXP operator !=(COL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.NotEqual };
        }

        public static BEXP operator >(COL op1, VAL op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.GreaterThan };
        }
        public static BEXP operator <(COL op1, VAL op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.LessThan };
        }
        public static BEXP operator <=(COL op1, VAL op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.LessThanOrEqual };
        }
        public static BEXP operator >=(COL op1, VAL op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.GreaterThanOrEqual };
        }
        public static BEXP operator ==(COL op1, VAL op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.Equal };
        }
        public static BEXP operator !=(COL op1, VAL op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.NotEqual };
        }



        public static TEXP operator +(COL op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Add };
        }
        public static TEXP operator -(COL op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Subtract };
        }
        public static TEXP operator *(COL op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Multiply };
        }
        public static TEXP operator /(COL op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Divide };
        }
        public static TEXP operator %(COL op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Modulo };
        }


        public NEXP this[string val]
        {
            get { return new NEXP { Name = val, Expression = this }; }
        }

        public override string ToString()
        {
            return Name;
        }
    }  // Column
    public class VAL : IQueryItem, IQueryValue
    {
        private object _value;

        public object Value
        {
            get { return _value ?? DBNull.Value; }
            set { _value = value; }
        }

        public VAL()
        {

        }
        public VAL(object value)
        {
            _value = value;
        }

        public static explicit operator VAL(string value)
        {
            return new VAL(value);
        }
        public static implicit operator VAL(int value)
        {
            return new VAL(value);
        }
        public static implicit operator VAL(float value)
        {
            return new VAL(value);
        }
        public static implicit operator VAL(double value)
        {
            return new VAL(value);
        }
        public static implicit operator VAL(bool value)
        {
            return new VAL(value);
        }
        public static implicit operator VAL(decimal value)
        {
            return new VAL(value);
        }
        public static implicit operator VAL(char value)
        {
            return new VAL(value);
        }
        public static implicit operator VAL(Guid value)
        {
            return new VAL(value);
        }
        public static implicit operator VAL(DateTime value)
        {
            return new VAL(value);
        }

        public static BEXP operator >(VAL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.GreaterThan };
        }
        public static BEXP operator <(VAL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.LessThan };
        }
        public static BEXP operator <=(VAL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.LessThanOrEqual };
        }
        public static BEXP operator >=(VAL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.GreaterThanOrEqual };
        }
        public static BEXP operator ==(VAL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.Equal };
        }
        public static BEXP operator !=(VAL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.NotEqual };
        }


        public static FEXP PI()
        {
            return new FEXP { Function = QueryFunctions.PI };
        }
        public static FEXP RAND()
        {
            return new FEXP { Function = QueryFunctions.Rand };
        }
        public static FEXP GETDATE()
        {
            return new FEXP { Function = QueryFunctions.GetDate };
        }


        public NEXP this[string val]
        {
            get { return new NEXP { Name = val, Expression = this }; }
        }

        public override string ToString()
        {
            return Value.ToString();
        }

    }  // Value
    public class ARR : IQueryItem
    {
        public IQueryValue[] Values { get; set; }

    }
    public class BEXP : IQueryItem, IExpression
    {
        public IQueryItem Operand1 { get; set; }
        public IQueryItem Operand2 { get; set; }
        public BinaryOperator Operator { get; set; }

        public static BEXP operator &(BEXP op1, BEXP op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.And };
        }
        public static BEXP operator |(BEXP op1, BEXP op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.Or };
        }
        public static BEXP operator !(BEXP op1)
        {
            return new BEXP { Operand1 = op1, Operator = BinaryOperator.Not };
        }

        public override string ToString()
        {
            switch (Operator)
            {
                case BinaryOperator.And: return string.Format("{0} and {1}", Operand1, Operand2);
                case BinaryOperator.Or: return string.Format("{0} or {1}", Operand1, Operand2);
                case BinaryOperator.Not: return string.Format("not {0}", Operand1);
                case BinaryOperator.Equal: return string.Format("{0} = {1}", Operand1, Operand2);
                case BinaryOperator.GreaterThan: return string.Format("{0} > {1}", Operand1, Operand2);
                case BinaryOperator.GreaterThanOrEqual: return string.Format("{0} >= {1}", Operand1, Operand2);
                case BinaryOperator.LessThan: return string.Format("{0} < {1}", Operand1, Operand2);
                case BinaryOperator.LessThanOrEqual: return string.Format("{0} <= {1}", Operand1, Operand2);
                case BinaryOperator.NotEqual: return string.Format("{0} != {1}", Operand1, Operand2);
                case BinaryOperator.Like: return string.Format("{0} like {1}", Operand1, Operand2);
                case BinaryOperator.In: return string.Format("{0} in {1}", Operand1, Operand2);
                default:
                    break;
            }
            return "";
        }


    } // Binary Expression
    public class TEXP : IQueryItem, IQueryValue, IExpression
    {
        public IQueryItem Operand1 { get; set; }
        public IQueryItem Operand2 { get; set; }
        public TransformOperator Operator { get; set; }

        public static BEXP operator >(TEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.GreaterThan };
        }
        public static BEXP operator <(TEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.LessThan };
        }
        public static BEXP operator <=(TEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.LessThanOrEqual };
        }
        public static BEXP operator >=(TEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.GreaterThanOrEqual };
        }
        public static BEXP operator ==(TEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.Equal };
        }
        public static BEXP operator !=(TEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.NotEqual };
        }


        public static TEXP operator +(TEXP op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Add };
        }
        public static TEXP operator -(TEXP op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Subtract };
        }
        public static TEXP operator *(TEXP op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Multiply };
        }
        public static TEXP operator /(TEXP op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Divide };
        }
        public static TEXP operator %(TEXP op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Modulo };
        }


        public NEXP this[string val]
        {
            get { return new NEXP { Name = val, Expression = this }; }
        }

        public override string ToString()
        {
            switch (Operator)
            {
                case TransformOperator.Add: return string.Format("{0} + {1}", Operand1, Operand2);
                case TransformOperator.Divide: return string.Format("{0} / {1}", Operand1, Operand2);
                case TransformOperator.Modulo: return string.Format("{0} % {1}", Operand1, Operand2);
                case TransformOperator.Multiply: return string.Format("{0} * {1}", Operand1, Operand2);
                case TransformOperator.Negate: return string.Format("-1 * {0}", Operand1);
                case TransformOperator.Power: return string.Format("{0} ^ {1}", Operand1, Operand2);
                case TransformOperator.Subtract: return string.Format("{0} - {1}", Operand1, Operand2);
                case TransformOperator.Lambda:
                    break;
                case TransformOperator.Conditional:
                    break;
                case TransformOperator.OnesComplement:
                    break;
                case TransformOperator.ExclusiveOr:
                    break;
                default:
                    break;
            }
            return string.Format("Operator: {0}, Operand1: {1}, Operand2: {2}", Operator, Operand1, Operand2);
        }

        //public static NEXP operator >>(TEXP op1, string op2)
        //{
        //    return new NEXP { Expression = op1, Name = op2 };
        //}

    } // Transform Expression
    public class FEXP : IQueryItem, IQueryValue, IExpression
    {
        public QueryFunctions Function { get; set; }
        public IQueryValue[] Parameters { get; set; }

        public NEXP this[string val]
        {
            get { return new NEXP { Name = val, Expression = this }; }
        }


        public static BEXP operator >(FEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.GreaterThan };
        }
        public static BEXP operator <(FEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.LessThan };
        }
        public static BEXP operator <=(FEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.LessThanOrEqual };
        }
        public static BEXP operator >=(FEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.GreaterThanOrEqual };
        }
        public static BEXP operator ==(FEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.Equal };
        }
        public static BEXP operator !=(FEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.NotEqual };
        }


        public static TEXP operator +(FEXP op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Add };
        }
        public static TEXP operator -(FEXP op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Subtract };
        }
        public static TEXP operator *(FEXP op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Multiply };
        }
        public static TEXP operator /(FEXP op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Divide };
        }
        public static TEXP operator %(FEXP op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Modulo };
        }

    } // Function Expression
    public class NEXP : IQueryItem, IQueryValue, INamedItem
    {
        public string Name { get; set; }
        public IQueryValue Expression { get; set; }

        public NEXP this[string val]
        {
            get { return new NEXP { Name = val, Expression = this }; }
        }

    } // Named Expression


    public class ASC : IQueryOrderItem
    {
        public IQueryValue Value { get; set; }
        public QueryOrderType Type { get { return QueryOrderType.ASC; } }

        public static explicit operator ASC(COL value)
        {
            return new ASC { Value = value };
        }
        public static explicit operator ASC(VAL value)
        {
            return new ASC { Value = value };
        }
        public static explicit operator ASC(TEXP value)
        {
            return new ASC { Value = value };
        }
        public static explicit operator ASC(NEXP value)
        {
            return new ASC { Value = value };
        }
    }
    public class DESC : IQueryOrderItem
    {
        public IQueryValue Value { get; set; }
        public QueryOrderType Type { get { return QueryOrderType.DESC; } }

        public static explicit operator DESC(COL value)
        {
            return new DESC { Value = value };
        }
        public static explicit operator DESC(VAL value)
        {
            return new DESC { Value = value };
        }
        public static explicit operator DESC(TEXP value)
        {
            return new DESC { Value = value };
        }
        public static explicit operator DESC(NEXP value)
        {
            return new DESC { Value = value };
        }
    }

    public static class QueryFunctionExp
    {
        public static BEXP LIKE(this IQueryValue op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.Like };
        }
        public static BEXP IN(this IQueryValue op1, params IQueryValue[] op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = new ARR { Values = op2.ToArray() }, Operator = BinaryOperator.In };
        }
        public static BEXP IN(this IQueryValue op1, IEnumerable<IQueryValue> op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = new ARR { Values = op2.ToArray() }, Operator = BinaryOperator.In };
        }
        //public static BEXP IN(this IQueryValue op1, IExecutable query)
        //{
        //    return new BEXP { Operator = BinaryOperator.In, Operand1 = op1, Operand2 = query };
        //}
        public static TEXP LOOKUP(this IQueryValue op, string returnValue, string destinationTable, string destinationColumn)
        {
            throw new NotImplementedException();
        }
        public static BEXP ISNULL(this IQueryValue op1)
        {
            return new BEXP { Operator = BinaryOperator.IsNull, Operand1 = op1 };
        }
        public static BEXP ISNOTNULL(this IQueryValue op1)
        {
            return new BEXP { Operator = BinaryOperator.IsNotNull, Operand1 = op1 };
        }


        /// <summary>
        /// String veya char ifadenin ilk karakterinin ASCII kodunu verir.
        /// </summary>
        /// <param name="character_expression">String, char alabilir. Diğer türler atanırsa string ifade  gibi işlen görür.</param>
        /// <returns>int</returns>
        public static FEXP ASCII(this IQueryValue character_expression)
        {
            return new FEXP { Function = QueryFunctions.Ascii, Parameters = new[] { character_expression } };
        }

        /// <summary>
        /// Verilen sayının ASCII karakter karşılığını verir. 255 ten sonra null döndürür.
        /// </summary>
        /// <param name="integer_expression">Sayısal değer. Noktalı sayı gelirse virgülden sonrası alınır.</param>
        /// <returns></returns>
        public static FEXP CHAR(this IQueryValue integer_expression)
        {
            return new FEXP { Function = QueryFunctions.Char, Parameters = new[] { integer_expression } };
        }

        /// <summary>
        /// Verilen ifadenin diğer ifade içindeki konumunu döndürür. MSSQL de stringin karakterlerini indexlemeye 1 den başlar.
        /// </summary>
        /// <param name="expressionToFind">Aradığımız ifade.</param>
        /// <param name="expressionToSearch">İfadeyi içinde aradığımız metin.</param>
        /// <returns></returns>
        public static FEXP CHARINDEX(this IQueryValue expressionToFind, IQueryValue expressionToSearch)
        {
            return new FEXP { Function = QueryFunctions.CharIndex, Parameters = new[] { expressionToFind, expressionToSearch } };
        }

        /// <summary>
        /// Verilen ifadenin diğer ifade içindeki konumunu döndürür. MSSQL de stringin karakterlerini indexlemeye 1 den başlar.
        /// </summary>
        /// <param name="expressionToFind">Aradığımız ifade.</param>
        /// <param name="expressionToSearch">İfadeyi içinde aradığımız metin.</param>
        /// <param name="start_location">Aramaya başlanacak konum.</param>
        /// <returns></returns>
        public static FEXP CHARINDEX(this IQueryValue expressionToFind, IQueryValue expressionToSearch, IQueryValue start_location)
        {
            return new FEXP { Function = QueryFunctions.CharIndex, Parameters = new[] { expressionToFind, expressionToSearch, start_location } };
        }

        /// <summary>
        /// Verilen string ifadeleri birleştirir.
        /// </summary>
        /// <param name="string_value1">String değer alır.</param>
        /// <param name="string_value2">String değer alır.</param>
        /// <param name="string_valueN">String değer listesi alır.</param>
        /// <returns></returns>
        public static FEXP CONCAT(this IQueryValue string_value1, IQueryValue string_value2, params IQueryValue[] string_valueN)
        {
            return new FEXP { Function = QueryFunctions.Concat, Parameters = new[] { string_value1, string_value2 }.Union(string_valueN).ToArray() };
        }

        /// <summary>
        /// Verilen iki karakterin SOUNDEX değerleri arasındaki farkı sayısal olarak verir.
        /// </summary>
        /// <param name="character_expression1">String ifade.</param>
        /// <param name="character_expression2">String ifade.</param>
        /// <returns>integer</returns>
        public static FEXP DIFFERENCE(this IQueryValue character_expression1, IQueryValue character_expression2)
        {
            return new FEXP { Function = QueryFunctions.Difference, Parameters = new[] { character_expression1, character_expression2 } };
        }

        /// <summary>
        /// Sayısal değerler ve tarihler için formatlama işlemi yapar.
        /// </summary>
        /// <param name="value">Formatlanacak değer.</param>
        /// <param name="format">Format.</param>
        /// <returns></returns>
        public static FEXP FORMAT(this IQueryValue value, IQueryValue format)
        {
            return new FEXP { Function = QueryFunctions.Format, Parameters = new[] { value, format } };
        }

        /// <summary>
        /// Sayısal değerler ve tarihler için formatlama işlemi yapar.
        /// </summary>
        /// <param name="value">Formatlanacak değer.</param>
        /// <param name="format">Format.</param>
        /// <param name="culture">Kültür seçimi.</param>
        /// <returns></returns>
        public static FEXP FORMAT(this IQueryValue value, IQueryValue format, IQueryValue culture)
        {
            return new FEXP { Function = QueryFunctions.Format, Parameters = new[] { value, format, culture } };
        }

        /// <summary>
        /// Soldan ikinci parametre ile belirtilen sayıda karakter döndürür.
        /// </summary>
        /// <param name="character_expression">String ifade.</param>
        /// <param name="integer_expression">Karakter sayısı.</param>
        /// <returns></returns>
        public static FEXP LEFT(this IQueryValue character_expression, IQueryValue integer_expression)
        {
            return new FEXP { Function = QueryFunctions.Left, Parameters = new[] { character_expression, integer_expression } };
        }

        /// <summary>
        /// String ifadenin uzunluğunu döndürür.
        /// </summary>
        /// <param name="string_expression">String ifade.</param>
        /// <returns></returns>
        public static FEXP LEN(this IQueryValue string_expression)
        {
            return new FEXP { Function = QueryFunctions.Len, Parameters = new[] { string_expression } };
        }

        /// <summary>
        /// Verilen string ifadeyi küçük harfe çevirir.
        /// </summary>
        /// <param name="character_expression">String ifade.</param>
        /// <returns></returns>
        public static FEXP LOWER(this IQueryValue character_expression)
        {
            return new FEXP { Function = QueryFunctions.Lower, Parameters = new[] { character_expression } };
        }

        /// <summary>
        /// Soldaki boşlukları siler.
        /// </summary>
        /// <param name="character_expression">String ifade.</param>
        /// <returns></returns>
        public static FEXP LTRIM(this IQueryValue character_expression)
        {
            return new FEXP { Function = QueryFunctions.Ltrim, Parameters = new[] { character_expression } };
        }

        /// <summary>
        /// baştaki ve sondaki boşlukları siler.
        /// </summary>
        /// <param name="character_expression">String ifade.</param>
        /// <returns></returns>
        public static FEXP TRIM(this IQueryValue character_expression)
        {
            return new FEXP { Function = QueryFunctions.Trim, Parameters = new[] { character_expression } };
        }

        /// <summary>
        /// Verilen sayının Unicode karakter karşılığını döndürür.
        /// </summary>
        /// <param name="integer_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP NCHAR(this IQueryValue integer_expression)
        {
            return new FEXP { Function = QueryFunctions.Nchar, Parameters = new[] { integer_expression } };
        }

        /// <summary>
        /// Verilen paternin ifade başlangıç indisini döndürür döndürür. Mssql de karakter indisleri 1 den başlar.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static FEXP PATINDEX(this IQueryValue pattern, IQueryValue expression)
        {
            return new FEXP { Function = QueryFunctions.Patindex, Parameters = new[] { pattern, expression } };
        }

        /// <summary>
        /// Returns a Unicode string with the delimiters added to make the input string a valid SQL Server delimited identifier.
        /// </summary>
        /// <param name="character_string">String ifade</param>
        /// <returns></returns>
        public static FEXP QUOTENAME(this IQueryValue character_string)
        {
            return new FEXP { Function = QueryFunctions.Quotename, Parameters = new[] { character_string } };
        }

        /// <summary>
        /// Returns a Unicode string with the delimiters added to make the input string a valid SQL Server delimited identifier.
        /// </summary>
        /// <param name="character_string">String ifade.</param>
        /// <param name="quote_character">Uzunluğu 1 olan string ifade.</param>
        /// <returns></returns>
        public static FEXP QUOTENAME(this IQueryValue character_string, IQueryValue quote_character)
        {
            return new FEXP { Function = QueryFunctions.Quotename, Parameters = new[] { character_string, quote_character } };
        }

        /// <summary>
        /// Verilen ifadeyi yeni ifade ile değiştirir.
        /// </summary>
        /// <param name="string_expression">String ifade.</param>
        /// <param name="string_pattern">Değiştirilecek ifade.</param>
        /// <param name="string_replacement">Eski ifade.</param>
        /// <returns></returns>
        public static FEXP REPLACE(this IQueryValue string_expression, IQueryValue string_pattern, IQueryValue string_replacement)
        {
            return new FEXP { Function = QueryFunctions.Replace, Parameters = new[] { string_expression, string_pattern, string_replacement } };
        }

        /// <summary>
        /// Verilen ifadeyi verilen sayıda tekrarlar.
        /// </summary>
        /// <param name="string_expression">ifade.</param>
        /// <param name="integer_expression">Tekrar sayısı.</param>
        /// <returns></returns>
        public static FEXP REPLICATE(this IQueryValue string_expression, IQueryValue integer_expression)
        {
            return new FEXP { Function = QueryFunctions.Replicate, Parameters = new[] { string_expression, integer_expression } };
        }

        /// <summary>
        /// Verilen string ifadeyi ters çevirir.
        /// </summary>
        /// <param name="string_expression">String ifade.</param>
        /// <returns></returns>
        public static FEXP REVERSE(this IQueryValue string_expression)
        {
            return new FEXP { Function = QueryFunctions.Reverse, Parameters = new[] { string_expression } };
        }

        /// <summary>
        /// Sağdan ikinci parametre ile belirtilen sayıda karakter döndürür.
        /// </summary>
        /// <param name="character_expression">String ifade.</param>
        /// <param name="integer_expression">Karakter sayısı.</param>
        /// <returns></returns>
        public static FEXP RIGHT(this IQueryValue character_expression, IQueryValue integer_expression)
        {
            return new FEXP { Function = QueryFunctions.Right, Parameters = new[] { character_expression, integer_expression } };
        }

        /// <summary>
        /// Sağdaki boşlukları siler.
        /// </summary>
        /// <param name="character_expression">String ifade.</param>
        /// <returns></returns>
        public static FEXP RTRIM(this IQueryValue character_expression)
        {
            return new FEXP { Function = QueryFunctions.Rtrim, Parameters = new[] { character_expression } };
        }

        /// <summary>
        /// Stringlerin benzerliğini değerlendirmek için dört karakterli (SOUNDEX) kod döndürür.
        /// </summary>
        /// <param name="character_expression">String ifade.</param>
        /// <returns></returns>
        public static FEXP SOUNDEX(this IQueryValue character_expression)
        {
            return new FEXP { Function = QueryFunctions.Soundex, Parameters = new[] { character_expression } };
        }

        /// <summary>
        /// Verilen sayıda boşluk üretir.
        /// </summary>
        /// <param name="integer_expression">Sayısal ifade.</param>
        /// <returns></returns>
        public static FEXP SPACE(this IQueryValue integer_expression)
        {
            return new FEXP { Function = QueryFunctions.Space, Parameters = new[] { integer_expression } };
        }

        /// <summary>
        /// Verilen sayısal ifadeyi stringe çevirir.
        /// </summary>
        /// <param name="float_expression">Sayısal ifade.</param>
        /// <returns></returns>
        public static FEXP STR(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Str, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Verilen sayısal ifadeyi stringe çevirir.
        /// </summary>
        /// <param name="float_expression">Sayısal ifade.</param>
        /// <param name="length">Toplam uzunluk. Varsayılan değer 10 dur. Eğer sayısal ifadenin uzunluğu toplam uzunluktan küçük ise sol tarafa boşluk ekler.</param>
        /// <returns></returns>
        public static FEXP STR(this IQueryValue float_expression, IQueryValue length)
        {
            return new FEXP { Function = QueryFunctions.Str, Parameters = new[] { float_expression, length } };
        }

        /// <summary>
        /// Verilen sayısal ifadeyi stringe çevirir.
        /// </summary>
        /// <param name="float_expression">Sayısal ifade.</param>
        /// <param name="length">Toplam uzunluk. Varsayılan değer 10 dur. Eğer sayısal ifadenin uzunluğu toplam uzunluktan küçük ise sol tarafa boşluk ekler.</param>
        /// <returns></returns>
        /// <param name="decima">Virgülden sonraki karakter sayısı.</param>
        /// <returns></returns>
        public static FEXP STR(this IQueryValue float_expression, IQueryValue length, IQueryValue decima)
        {
            return new FEXP { Function = QueryFunctions.Str, Parameters = new[] { float_expression, length, decima } };
        }

        /// <summary>
        /// MSSQL2016 da çalışıyor.
        /// Escapes special characters in texts and returns text with escaped characters. STRING_ESCAPE is a deterministic function.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static FEXP STRING_ESCAPE(this IQueryValue text, IQueryValue type)
        {
            return new FEXP { Function = QueryFunctions.String_Escape, Parameters = new[] { text, type } };
        }

        /// <summary>
        /// MSSQL2016 da çalışıyor.
        /// String ifadeyi verilen karaktere göre böler.
        /// Splits the character expression using specified separator.
        /// </summary>
        /// <param name="string_expression"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static FEXP STRING_SPLIT(this IQueryValue string_expression, IQueryValue separator)
        {
            return new FEXP { Function = QueryFunctions.String_Split, Parameters = new[] { string_expression, separator } };
        }

        /// <summary>
        /// Başlangıç ve uzunluk değeri ile belirtilen yere yeni string eklenir.
        /// </summary>
        /// <param name="character_expression">String ifade.</param>
        /// <param name="start">Başlangıç.</param>
        /// <param name="length">Uzunluk.</param>
        /// <param name="replaceWith_expression">Yeni ifade.</param>
        /// <returns></returns>
        public static FEXP STUFF(this IQueryValue character_expression, IQueryValue start, IQueryValue length, IQueryValue replaceWith_expression)
        {
            return new FEXP { Function = QueryFunctions.Stuff, Parameters = new[] { character_expression, start, length, replaceWith_expression } };
        }

        /// <summary>
        /// String ifade içinde başlangıç ve uzunluk değerleri ile ifade edilen konumdaki ifadeyi döndürür.
        /// </summary>
        /// <param name="expression">String ifade.</param>
        /// <param name="start">Başlangıç.</param>
        /// <returns></returns>
        public static FEXP SUBSTRING(this IQueryValue expression, IQueryValue start)
        {
            return new FEXP { Function = QueryFunctions.Substring, Parameters = new[] { expression, start } };
        }

        /// <summary>
        /// String ifade içinde başlangıç ve uzunluk değerleri ile ifade edilen konumdaki ifadeyi döndürür.
        /// </summary>
        /// <param name="expression">String ifade.</param>
        /// <param name="start">Başlangıç.</param>
        /// <param name="length">Uzunluk.</param>
        /// <returns></returns>
        public static FEXP SUBSTRING(this IQueryValue expression, IQueryValue start, IQueryValue length)
        {
            return new FEXP { Function = QueryFunctions.Substring, Parameters = new[] { expression, start, length } };
        }

        /// <summary>
        /// String veya char ifadenin ilk karakterinin Unicode kodunu verir.
        /// </summary>
        /// <param name="ncharacter_expression">String, char alabilir. Diğer türler atanırsa string ifade  gibi işlen görür.</param>
        /// <returns></returns>
        public static FEXP UNICODE(this IQueryValue ncharacter_expression)
        {
            return new FEXP { Function = QueryFunctions.Unicode, Parameters = new[] { ncharacter_expression } };
        }

        /// <summary>
        /// Verilen string ifadeyi büyük harfe çevirir.
        /// </summary>
        /// <param name="character_expression">String ifade.</param>
        /// <returns></returns>
        public static FEXP UPPER(this IQueryValue character_expression)
        {
            return new FEXP { Function = QueryFunctions.Upper, Parameters = new[] { character_expression } };
        }




        /// <summary>
        /// Akosinüs işlemi. Radyan değer döndürür.
        /// </summary>
        /// <param name="float_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP ACOS(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Acos, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Asinüs işlemi. Radyan değer döndürür.
        /// </summary>
        /// <param name="float_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP ASIN(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Asin, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Atanjant işlemi. Radyan değer döndürür.
        /// </summary>
        /// <param name="float_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP ATAN(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Atan, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Atanjant2 işlemi. Radyan değer döndürür.
        /// </summary>
        /// <param name="float_expression1">Sayısal değer.</param>
        /// <param name="float_expression2">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP ATN2(this IQueryValue float_expression1, IQueryValue float_expression2)
        {
            return new FEXP { Function = QueryFunctions.Atn2, Parameters = new[] { float_expression1, float_expression2 } };
        }

        /// <summary>
        /// Kosinüs işlemi. Radyan değer alır.
        /// </summary>
        /// <param name="float_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP COS(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Cos, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Kotanjant işlemi. Radyan değer alır.
        /// </summary>
        /// <param name="float_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP COT(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Cot, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Sinüs fonksiyonu. Radyan değer alır.
        /// </summary>
        /// <param name="float_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP SIN(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Sin, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Tanjant işlemi. Radyan değer alır.
        /// </summary>
        /// <param name="float_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP TAN(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Tan, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Radyan olarak verilen değer dereceye çevirir.
        /// </summary>
        /// <param name="numeric_expression">Radyan değer.</param>
        /// <returns></returns>
        public static FEXP DEGREES(this IQueryValue numeric_expression)
        {
            return new FEXP { Function = QueryFunctions.Degrees, Parameters = new[] { numeric_expression } };
        }

        /// <summary>
        /// Derece olarak verilen değeri radyana çevirir.
        /// </summary>
        /// <param name="numeric_expression">Derece değer.</param>
        /// <returns></returns>
        public static FEXP RADIANS(this IQueryValue numeric_expression)
        {
            return new FEXP { Function = QueryFunctions.Radians, Parameters = new[] { numeric_expression } };
        }


        /// <summary>
        /// Verilen sayısal değerin mutlak değerini alır.
        /// </summary>
        /// <param name="numeric_expression">Sayısal değer</param>
        /// <returns></returns>
        public static FEXP ABS(this IQueryValue numeric_expression)
        {
            return new FEXP { Function = QueryFunctions.Abs, Parameters = new[] { numeric_expression } };
        }

        /// <summary>
        /// Yukarı yuvarlama.
        /// </summary>
        /// <param name="numeric_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP CEILING(this IQueryValue numeric_expression)
        {
            return new FEXP { Function = QueryFunctions.Ceiling, Parameters = new[] { numeric_expression } };
        }

        /// <summary>
        /// Aşağı yuvarlama.
        /// </summary>
        /// <param name="numeric_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP FLOOR(this IQueryValue numeric_expression)
        {
            return new FEXP { Function = QueryFunctions.Floor, Parameters = new[] { numeric_expression } };
        }

        /// <summary>
        /// Yuvarlama işlemi
        /// </summary>
        /// <param name="numeric_expression">Yuvarlancak sayı.</param>
        /// <returns></returns>
        public static FEXP ROUND(this IQueryValue numeric_expression)
        {
            return new FEXP { Function = QueryFunctions.Round, Parameters = new[] { numeric_expression } };
        }

        /// <summary>
        /// Yuvarlama işlemi
        /// </summary>
        /// <param name="numeric_expression">Yuvarlancak sayı.</param>
        /// <param name="length">Virgülden sonraki basamak sayısı.</param>
        /// <returns></returns>
        public static FEXP ROUND(this IQueryValue numeric_expression, IQueryValue length)
        {
            return new FEXP { Function = QueryFunctions.Round, Parameters = new[] { numeric_expression, length } };
        }

        /// <summary>
        /// Yuvarlama işlemi.
        /// </summary>
        /// <param name="numeric_expression">Yuvarlancak sayı.</param>
        /// <param name="length">Virgülden sonraki basamak sayısı.</param>
        /// <param name="truncate">0 - ise yuvarlar, diğer sayılarda keser.</param>
        /// <returns></returns>
        public static FEXP ROUND(this IQueryValue numeric_expression, IQueryValue length, IQueryValue truncate)
        {
            return new FEXP { Function = QueryFunctions.Round, Parameters = new[] { numeric_expression, length, truncate } };
        }

        /// <summary>
        /// İşaret fonksiyonu.
        /// </summary>
        /// <param name="numeric_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP SIGN(this IQueryValue numeric_expression)
        {
            return new FEXP { Function = QueryFunctions.Sign, Parameters = new[] { numeric_expression } };
        }


        /// <summary>
        /// e^x işlemi
        /// </summary>
        /// <param name="float_expression">Üs. Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP EXP(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Exp, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Doğal logaritma işlemi.
        /// </summary>
        /// <param name="float_expression">Logaritması alınacak sayı.</param>
        /// <returns></returns>
        public static FEXP LOG(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Log, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Logaritma işlemi
        /// </summary>
        /// <param name="float_expression">Logaritması alınacak sayı.</param>
        /// <param name="base1">Taban.</param>
        /// <returns></returns>
        public static FEXP LOG(this IQueryValue float_expression, IQueryValue base1)
        {
            return new FEXP { Function = QueryFunctions.Log, Parameters = new[] { float_expression, base1 } };
        }

        /// <summary>
        /// Üs alma işlemi.
        /// </summary>
        /// <param name="float_expression">Üs alınacak sayı.</param>
        /// <param name="y">Üs.</param>
        /// <returns></returns>
        public static FEXP POWER(this IQueryValue float_expression, IQueryValue y)
        {
            return new FEXP { Function = QueryFunctions.Power, Parameters = new[] { float_expression, y } };
        }

        /// <summary>
        /// Karekök işlemi.
        /// </summary>
        /// <param name="float_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP SQRT(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Sqrt, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Kare işlemi
        /// </summary>
        /// <param name="float_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP SQUARE(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Square, Parameters = new[] { float_expression } };
        }






        public static FEXP AVG(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Avg, Parameters = new[] { expression } }; }
        public static FEXP CHECKSUM_AGG(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Checksum_Agg, Parameters = new[] { expression } }; }
        public static FEXP COUNT(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Count, Parameters = new[] { expression } }; }
        public static FEXP COUNT_BIG(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Count_Big, Parameters = new[] { expression } }; }
        public static FEXP GROUPING(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Grouping, Parameters = new[] { expression } }; }
        public static FEXP GROUPING_ID(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Grouping_Id, Parameters = new[] { expression } }; }
        public static FEXP MAX(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Max, Parameters = new[] { expression } }; }
        public static FEXP MIN(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Min, Parameters = new[] { expression } }; }
        public static FEXP SUM(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Sum, Parameters = new[] { expression } }; }
        public static FEXP STDEV(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Stdev, Parameters = new[] { expression } }; }
        public static FEXP STDEVP(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Stdevp, Parameters = new[] { expression } }; }
        public static FEXP VAR(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Var, Parameters = new[] { expression } }; }
        public static FEXP VARP(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Varp, Parameters = new[] { expression } }; }

    }

}
