using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ScoopFramework.Enum;
using ScoopFramework.LogControl;
using ScoopFramework.Model;

namespace ScoopFramework.DataLayer.Proses.Db
{
    public struct DataProcessingDelete
    {
        /// <summary>
        /// //DELETE FROM TABLO WHERE ID=1
        /// </summary>
        /// <param name="paramValues"></param>
        /// <param name="dbTablo"></param>
        /// <param name="connection"></param>
        public static void Delete(List<ParamValue> paramValues, string dbTablo,
            SqlConnection connection)
        {
            using (var command = new SqlCommand())
            {
                try
                {
                    var table = dbTablo.GetType().Name != DBDataTypeEnum.String.ToString() ? dbTablo.GetType().Name : dbTablo;

                    var sql = string.Format("{0} {1} {2}", SqlQueryEnum.Delete, SqlQueryEnum.FROM, table);

                    sql = paramValues.Where(whr => whr.Where != null).SelectMany(
                                    whr => whr.Where)
                                    .Aggregate(sql,
                                        (current, item) => current + string.Format(" {0} {1}={2}", SqlQueryEnum.WHERE, item.Key, item.Value));

                    command.CommandText = sql;

                    command.Connection = connection;

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, false, false);
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
