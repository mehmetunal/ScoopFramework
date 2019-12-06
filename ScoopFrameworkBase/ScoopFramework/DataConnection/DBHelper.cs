using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ScoopFramework.DataConnection
{
    public static class DBHelper
    {
        /// <summary>
        /// ADO.SQLCONNECTION
        /// </summary>
        /// <returns></returns>
        public static SqlConnection Connection()
        {
            try
            {
                return new SqlConnection(ConnectionType.LocalConnetion);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("connection :{0}", ex.Message));
            }
        }

        /// <summary>
        /// FORM.CS SQLCONNECTION
        /// </summary>
        /// <returns></returns>
        public static ConnectionStringSettings AppConnection()
        {
            try
            {
                return ConfigurationManager.ConnectionStrings["AypiAjans"];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("connection :{0}", ex.Message));
            }
            return new ConnectionStringSettings();
        }
    }
}
