using ScoopFramework.Helper;
using ScoopFramework.MySql;
using System.Data.Common;
using System.Web.Configuration;

namespace ScoopFramework.DataBussiens
{
    public partial class ScoopDatabase
    {

        public string ConnectionString { get; private set; }
        public string DbName { get; private set; }
        public ScoopDatabase()
        {
            this.ConnectionString = WebConfigurationManager.AppSettings["DBConnection"];
            this.DbName = WebConfigurationManager.AppSettings["DBShema"];
        }

        public ScoopDatabase(string conn, string dbName)
        {

            try
            {
                this.ConnectionString = WebConfigurationManager.AppSettings["DBConnection"];
                this.DbName = WebConfigurationManager.AppSettings["DBShema"];
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(string.Format("connection :{0}", ex.Message));
            }
        }


        public Providerlator GetDB(DbTransaction tran = null)
        {
            return new Providerlator(ConnectionString, tran);
        }

        public DbTransaction BeginTransaction()
        {
            return new Providerlator(ConnectionString).BeginTransaction();
        }


        public void Dispose()
        {
            new Providerlator(ConnectionString).Dispose();
        }

        public void Close()
        {
            new Providerlator(ConnectionString).Close();
        }

        public static class ScoopConnectionType
        {
            public static string ConnectionString = WebConfigurationManager.AppSettings["DBConnection"];
        }
        public static class ScoopDBShema
        {
            public static string DbName = WebConfigurationManager.AppSettings["DBConnection"];
        }
        //public static Providerlator ProviderConnection(DbTransaction transaction = null)
        //{
        //    try
        //    {
        //        return new Providerlator(WebConfigConnection, transaction);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw new System.Exception(string.Format("connection :{0}", ex.Message));
        //    }
        //}
        public MySqlProviderlator MySqlProviderConnection()
        {
            try
            {
                return new MySqlProviderlator(ScoopConnectionType.ConnectionString);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(string.Format("connection :{0}", ex.Message));
            }
        }
    }

}