using System;

namespace ScoopFramework.Interface
{
    public interface ITypeMapper
    {
        Type GetType(string sqlType);
        string GetSqlType(Type type, int? length = null);
        string FormatSqlByType(object val);
        object ConvertFromSql(object obj);
        object ConvertToSql(object obj);
    }
}
