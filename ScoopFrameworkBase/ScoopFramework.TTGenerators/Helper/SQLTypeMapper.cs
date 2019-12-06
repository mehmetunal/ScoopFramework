using System;
using System.Collections.Generic;

namespace ScoopFramework.TTGenerators.Helper
{
    class SQLTypeMapper
    {
        //https://msdn.microsoft.com/en-us/library/cc716729(v=vs.110).aspx
        static Dictionary<Type, string> _types = new Dictionary<Type, string>();

        static SQLTypeMapper()
        {
            _types[typeof(long)] = "bigint";
            _types[typeof(long?)] = "bigint";
            //binary Byte[]
            _types[typeof(bool)] = "bit";
            _types[typeof(bool?)] = "bit";
            _types[typeof(char)] = "char";
            _types[typeof(char?)] = "char";
            _types[typeof(DateTime)] = "datetime";
            _types[typeof(DateTime?)] = "datetime";
            _types[typeof(DateTimeOffset)] = "datetimeoffset";
            _types[typeof(DateTimeOffset?)] = "datetimeoffset";
            _types[typeof(decimal)] = "decimal";
            _types[typeof(decimal?)] = "decimal";
            //FILESTREAM attribute (varbinary(max)) Byte[]
            _types[typeof(double)] = "float";
            _types[typeof(double?)] = "float";
            //image Byte[]
            _types[typeof(int)] = "int";
            _types[typeof(int?)] = "int";
            //money Decimal
            //nchar String
            //ntext String
            //numeric Decimal
            _types[typeof(string)] = "nvarchar(MAX)";
            //real Single
            //rowversion Byte[]
            //smalldatetime DateTime
            _types[typeof(short)] = "smallint";
            _types[typeof(short?)] = "smallint";
            //smallmoney Decimal
            //sql_variant Object*
            //text String
            _types[typeof(TimeSpan)] = "time";
            _types[typeof(TimeSpan?)] = "time";
            //timestamp Byte[]
            _types[typeof(byte)] = "tinyint";
            _types[typeof(byte?)] = "tinyint";
            _types[typeof(Guid)] = "uniqueidentifier";
            _types[typeof(Guid?)] = "uniqueidentifier";
            _types[typeof(byte[])] = "varbinary(max)";
            //varchar String
            //xml Xml
        }

        public static string GetSQLType<T>()
        {
            return GetSQLType(typeof(T));
        }

        public static string GetSQLType(Type t)
        {
            string result = null;
            _types.TryGetValue(t, out result);

            if (ReflectionHelper.IsEnum(t))
                return "nvarchar(100)";

            if (result == null) result = "";
            return result;
        }

    }
}
