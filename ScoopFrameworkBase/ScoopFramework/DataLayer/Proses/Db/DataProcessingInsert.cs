using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ScoopFramework.Enum;
using ScoopFramework.LogControl;
using ScoopFramework.Model;

namespace ScoopFramework.DataLayer.Proses.Db
{
    public class DataProcessingInsert
    {
        public static object Insert(object dbTablo, string param, string value, SqlConnection connection,
            List<ParamValue> paramValues)
        {
            var table = dbTablo.GetType().Name != DBDataTypeEnum.String.ToString()
                ? dbTablo.GetType().Name
                : dbTablo.ToString();
            var sql = string.Format("{0} {1} {2} ({3}) {4} ({5}) SELECT SCOPE_IDENTITY()", SqlQueryEnum.INSERT, SqlQueryEnum.INTO, table, param,
                SqlQueryEnum.VALUES, value);

            using (var command = new SqlCommand(sql, connection))
            {
                var transaction = connection.BeginTransaction("ScoopTransaction");
                // Start a local transaction.
                command.Transaction = transaction;

                try
                {
                    var lgParamValue = "";
                    foreach (var item in paramValues)
                    {
                        var prm = string.Format("@{0}", item.Key);
                        command.Parameters.AddWithValue(prm, item.Value ?? string.Empty);

                        lgParamValue += item.Value ?? "";
                    }


                    var geriDonusId = command.ExecuteScalar();

                    // Attempt to commit the transaction.
                    transaction.Commit();

                    var lgSql = string.Format("{0} {1} {2} ({3}) {4} ({5})", SqlQueryEnum.INSERT, SqlQueryEnum.INTO, table, param,
                      SqlQueryEnum.VALUES, lgParamValue);
                    Logger.Log(lgSql, false, true);

                    return geriDonusId;
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        var logMessage = string.Format("Rollback Exception Type: {0} , Message: {1}", ex2.GetType(), ex2.Message);
                        Logger.Log(logMessage, false, false);
                    }
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
