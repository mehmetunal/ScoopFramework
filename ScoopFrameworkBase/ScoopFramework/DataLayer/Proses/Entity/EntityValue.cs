using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using ScoopFramework.DataLayer.Library;
using ScoopFramework.Extension;
using ScoopFramework.Model;

namespace ScoopFramework.DataLayer.Proses.Entity
{
    public static class EntityValue<T> where T
          : new()
    {
        public static void GetValue(T entity, List<PropertyInfo> props, ref string param, ref string value,
           ref List<ParamValue> paramValues)
        {
            var type = typeof(T);
            var pkProperty = type.GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).Length > 0)
                .FirstOrDefault();
            props.Remove(pkProperty);

            param = string.Join(",", props.Select(p => string.Format("{0}", p.Name)));
            value = string.Join(",", props.Select(p => string.Format("@{0}", p.Name)));

            paramValues.AddRange(
                props.Select(prop => new ParamValue()
                {
                    Key = prop.Name,
                    Value = prop.GetValue(entity, null)
                }));

        }
        public static void GetValueRow(T entity, System.Data.DataTable dTable, Type type, ref DataRow row)
        {

            foreach (var columnName in dTable.Columns.Cast<DataColumn>().Select(cl => cl.ColumnName))
            {
                if (type.GetProperty(columnName) == null && type.GetProperty(columnName).GetValue(entity, null) == null)
                {
                    continue;
                }

                var propertyValue = type.GetProperty(columnName).GetValue(entity, null);

                var property = typeof(T).GetProperty(columnName) ?? entity.GetType().GetProperty(columnName);

                var propertyType = property.PropertyType;

                if (propertyType.Name == "DateTime")
                {
                    var defaulValue = property.PropertyType.GetDefault();

                    if (!row[columnName].Equals(propertyValue) && propertyValue != null && !defaulValue.Equals(propertyValue))
                    {
                        row[columnName] = propertyValue;
                        continue;
                    }
                    var dateValue = dTable.Rows[0][columnName];
                    row[columnName] = dateValue;
                    continue;
                }
                if (!row[columnName].Equals(propertyValue) && propertyValue != null)
                {
                    row[columnName] = propertyValue;
                    continue;
                }

            }

        }
    }
}
