using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ScoopFramework.DataLayer.Proses.Properties
{
    public static class PropertiesTransactions<T> where T
         : new()
    {
        public static IEnumerable<PropertyInfo> DefaulRemoveValueProperty(T entity, object tablo = null)
        {
            var gercekTabloProp = GercekTabloProp(tablo);

            var prop = new List<PropertyInfo>();
            foreach (var propertyInfo in gercekTabloProp)
            {
                foreach (var p in entity.GetType().GetProperties().Where(info => info.Name.Equals(propertyInfo.Name)))
                {
                    if (propertyInfo.Name == "id" || propertyInfo.Name == "Id") continue;
                    if (p.GetValue(entity, null) == null) continue;
                    if (propertyInfo.PropertyType.Name == "DateTime")
                    {
                        var defaulDate = default(DateTime).ToString("yy-MM-dd");

                        var dateTime = Convert.ToDateTime(propertyInfo.GetValue(entity, null).ToString());

                        var itemValue = string.Format("{0:yy-MM-dd}", dateTime);

                        if (defaulDate != itemValue)
                        {
                            prop.Add(entity.GetType().GetProperties().FirstOrDefault(info => info.Name == propertyInfo.Name));
                        }
                        continue;
                    }
                    prop.Add(entity.GetType().GetProperties().FirstOrDefault(info => info.Name == propertyInfo.Name));
                }

            }
            return prop;
        }
        public static List<PropertyInfo> GercekTabloProp(object tablo)
        {
            /*AppDomaime *.dll'leri yükler*/
            //PreLoadDeployedAssemblies();
            var dbClassFromString = MethodHelper.GetDbClassFromString(tablo.ToString());
            List<PropertyInfo> prop = dbClassFromString.GetProperties().ToList();
            return prop;
        }
    }
}
