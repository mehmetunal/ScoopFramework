using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ScoopFramework.Enum;

namespace ScoopFramework.DataLayer.Proses.DataTable
{
    public struct DataTableConvert
    {
        /// <summary>
        /// The data table to list.
        /// </summary>
        /// <param name="dataTable">
        /// The data table.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static List<T> DataTableToList<T>(System.Data.DataTable dataTable) where T : new()
        {

            var listItem = new List<T>();
            if (dataTable.Rows.Count > 0)
            {
                var tClass = typeof(T);
                var pClass = tClass.GetProperties();
                var dc = dataTable.Columns.Cast<DataColumn>().ToList();
                foreach (DataRow item in dataTable.Rows)
                {
                    var cn = (T)Activator.CreateInstance(tClass);
                    foreach (var pc in pClass)
                    {
                        try
                        {
                            var d = dc.Find(c => c.ColumnName == pc.Name);
                            if (d != null)
                            {
                                var type00 = item[pc.Name].GetType().Name;
                                //TODO:BURASI DUZELTILECEK;
                                if (type00 == DBDataTypeEnum.Int32.ToString())
                                {
                                    pc.SetValue(cn, item[pc.Name], null);
                                }
                                else if (type00 == DBDataTypeEnum.DBNull.ToString())
                                {
                                    pc.SetValue(cn, (item[pc.Name] == DBNull.Value) ? null : item[pc.Name], null);
                                }
                                else if (type00 == DBDataTypeEnum.DateTime.ToString())
                                {
                                    //var a = String.Format("{0:dd/MM/yyyy}", item[pc.Name]);

                                    var date = Convert.ToDateTime(item[pc.Name], System.Threading.Thread.CurrentThread.CurrentCulture);
                                    //var 
                                    //var col = DateTime.ParseExact(item[pc.Name], format, null);

                                    if (item[pc.Name] == DBNull.Value)
                                    {
                                        pc.SetValue(cn, DateTime.Now, null);
                                    }
                                    else
                                    {
                                        pc.SetValue(cn, date, null);
                                    }

                                }
                                else if (type00 == DBDataTypeEnum.SqlGeography.ToString())
                                {
                                    pc.SetValue(cn, null, null);
                                }
                                else
                                {
                                    pc.SetValue(cn, (item[pc.Name] == DBNull.Value) ? string.Empty : item[pc.Name], null);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message); //"DataTable Liste çevrilemedi.");
                        }
                    }

                    listItem.Add(cn);
                }
            }

            return listItem;
        }
    }
}
