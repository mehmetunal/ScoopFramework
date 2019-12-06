using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using ScoopFramework.DBModel;

namespace ScoopFramework.Extension
{
    public static class ExtensionsMethod
    {
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string ToDescription(this System.Enum item)
        {
            MemberInfo[] memberInfos = item.GetType().GetMembers(BindingFlags.Public | BindingFlags.Static);

            var res = memberInfos.Where(a => a.Name == item.ToString()).FirstOrDefault();

            if (res != null)
            {
                return res.GetCustomAttributesData().Select(a => a.ConstructorArguments).FirstOrDefault().FirstOrDefault().Value.ToString();
            }
            else
            {
                return item.ToString();
            }

        }
        public static T[] ToSelectGrid<T>(this T[] data)
        {

            if (data != null)
            {
                foreach (var dataRow in data)
                {
                    var objProps = dataRow.GetType().GetProperties();

                    foreach (var propertyInfo in objProps.Where(info => info.PropertyType.FullName.Equals("Microsoft.SqlServer.Types.SqlGeography")))
                    {
                        propertyInfo.SetValue(dataRow, null, null);
                    }
                }

                return data.ToArray();

            }

            return new T[] { };

        }

        public static T ToSelectGrid<T>(this T data) where T : new()
        {
            if (data != null)
            {
                var propType = data.GetType().GetProperties();
                foreach (var propertyInfo in propType.Where(info => info.PropertyType.FullName.Equals("Microsoft.SqlServer.Types.SqlGeography")))
                {
                    propertyInfo.SetValue(data, null, null);
                }
                return data;
            }

            return data;
        }
        public static object GetDefault(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        public static DataTable ConvertToDataTable<T>(this IEnumerable<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
        public static bool IsValidGuid(this string str)
        {
            Guid guid;
            return Guid.TryParse(str, out guid);
        }

        public static bool IsNullOrEmptyOrGuid(this string param)
        {
            return (String.IsNullOrEmpty(param) || !param.IsValidGuid());
        }

        public static string TrimStartAndEnd(this string trim)
        {
            var value = String.Empty;
            if (!String.IsNullOrEmpty(trim))
            {
                value = trim.TrimStart();
            }
            return value.TrimEnd();
        }

        public static string TrimStart(this string trim)
        {
            return !String.IsNullOrEmpty(trim) ? trim.TrimStart() : String.Empty;
        }

        public static string TrimEnd(this string trim)
        {
            return !String.IsNullOrEmpty(trim) ? trim.TrimEnd() : String.Empty;
        }

        public static string TrimSafeEnd(this string input, string suffixToRemove)
        {
            if (input != null && suffixToRemove != null && input.EndsWith(suffixToRemove))
            {
                return input.Substring(0, input.Length - suffixToRemove.Length);
            }
            return input;
        }

        public static double ToDiscount(this double totalPrice, double discount)
        {
            if (discount != default(double))
            {
                var result = (totalPrice / 100 * discount);
                return result;
            }
            return totalPrice;

        }

        public static double ToPriceDecalDiscount(this double totalPrice, double price)
        {
            return totalPrice - price;
        }
        public static bool IsNumeric(this string text)
        {
            double oReturn = 0;
            return !String.IsNullOrEmpty(text) && Double.TryParse(text, out oReturn);
        }
        public static bool ExportExcel(this DataTable dataTable, string name = "Excel-Export")
        {
            try
            {
                var rnd = new Random().Next(0, 10);
                var attachment = "attachment; filename=" + name + ".xls";
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.Buffer = true;
                //HttpContext.Current.Response.Charset = "UTF-8";
                HttpContext.Current.Response.AddHeader("content-disposition", attachment);
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                var tab = "";
                foreach (DataColumn dc in dataTable.Columns)
                {
                    HttpContext.Current.Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                HttpContext.Current.Response.Write("\n");
                foreach (DataRow dr in dataTable.Rows)
                {
                    tab = "";
                    int i;
                    for (i = 0; i < dataTable.Columns.Count; i++)
                    {
                        HttpContext.Current.Response.Write(tab + dr[i]);
                        tab = "\t";
                    }
                    HttpContext.Current.Response.Write("\n");
                }
                HttpContext.Current.Response.End();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
