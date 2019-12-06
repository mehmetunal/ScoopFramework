using System;
using System.Collections.Generic;
using System.Data;

namespace Infoline.Framework.CodeGeneration.CodeGenerators
{
    public class SqlTypeConverter
    {

        Dictionary<Type, string> _alias = new Dictionary<Type, string>();
        Dictionary<SqlDbType, Type> _types = new Dictionary<SqlDbType, Type>();
        Dictionary<string, Type> _stypes = new Dictionary<string, Type>();

        public SqlTypeConverter()
        {
            _alias[typeof(string)] = "string";
            _alias[typeof(long)] = "long";
            _alias[typeof(char)] = "char";
            _alias[typeof(byte)] = "byte";
            _alias[typeof(int)] = "int";
            _alias[typeof(bool)] = "bool";
            _alias[typeof(decimal)] = "decimal";
            _alias[typeof(double)] = "double";
            _alias[typeof(short)] = "short";
            _alias[typeof(ushort)] = "ushort";
            _alias[typeof(ulong)] = "ulong";
            _alias[typeof(uint)] = "uint";
            _alias[typeof(byte[])] = "byte []";

            _types[SqlDbType.BigInt] = typeof(long);
            _types[SqlDbType.Binary] = typeof(byte[]);
            _types[SqlDbType.Bit] = typeof(bool);
            _types[SqlDbType.Char] = typeof(string);
            _types[SqlDbType.Date] = typeof(DateTime);
            _types[SqlDbType.DateTime] = typeof(DateTime);
            _types[SqlDbType.DateTime2] = typeof(DateTime);
            _types[SqlDbType.DateTimeOffset] = typeof(DateTime);
            _types[SqlDbType.Decimal] = typeof(decimal);
            _types[SqlDbType.Float] = typeof(double);
            _types[SqlDbType.Binary] = typeof(byte[]);
            _types[SqlDbType.UniqueIdentifier] = typeof(Guid);
            _types[SqlDbType.Int] = typeof(int);
            _types[SqlDbType.Image] = typeof(byte[]);

            _types[SqlDbType.Money] = typeof(decimal);
            _types[SqlDbType.NChar] = typeof(string);
            _types[SqlDbType.NText] = typeof(string);
            _types[SqlDbType.NVarChar] = typeof(string);
            _types[SqlDbType.Real] = typeof(float);
            _types[SqlDbType.SmallDateTime] = typeof(DateTime);
            _types[SqlDbType.SmallInt] = typeof(short);
            _types[SqlDbType.TinyInt] = typeof(byte);
            _types[SqlDbType.SmallMoney] = typeof(decimal);
            _types[SqlDbType.Text] = typeof(string);
            _types[SqlDbType.Time] = typeof(DateTime);
            _types[SqlDbType.Timestamp] = typeof(byte[]);
            _types[SqlDbType.VarBinary] = typeof(byte[]);


            _types[SqlDbType.VarBinary] = typeof(byte[]);
            _types[SqlDbType.VarChar] = typeof(string);
            _types[SqlDbType.Variant] = typeof(object);
            _types[SqlDbType.Xml] = typeof(string);
            foreach (var k in _types)
            {
                _stypes[k.Key.ToString().ToLower(System.Globalization.CultureInfo.InvariantCulture)] = k.Value;
            }
        }

        public Type Convert(string type)
        {
            return _stypes[type];
        }

        public string GetAlias(Type stype)
        {
            string alias = stype.Name;
            if (!_alias.TryGetValue(stype, out alias))
                alias = stype.Name;
            return alias;
        }
    }
}
