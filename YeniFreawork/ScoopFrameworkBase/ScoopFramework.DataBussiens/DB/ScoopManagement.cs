using ScoopFramework.Helper;
using System.Data.Common;
using System.Web.Configuration;

namespace ScoopFramework.DataBussiens
{
    public partial class ScoopManagement
    {

        protected string ConnectionString { get; private set; }
        protected string DbName { get; private set; }
        public ScoopManagement()
        {
            this.ConnectionString = WebConfigurationManager.AppSettings["DBConnection"];

            this.DbName = WebConfigurationManager.AppSettings["DBShema"];
        }

        public ScoopManagement(string conn, string dbName)
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


        //public Providerlator GetDB(DbTransaction tran = null)
        //{
        //    return new Providerlator(ConnectionString, tran);
        //}

        //public DbTransaction BeginTransaction()
        //{
        //    return new Providerlator(ConnectionString).BeginTransaction();
        //}

        public ScoopDatabase<T> GetDB<T>(DbTransaction tran = null)
        {
            return new ScoopDatabase<T>(ConnectionString, tran);
        }

        public DbTransaction BeginTransaction()
        {
            return new ScoopDatabase<dynamic>(ConnectionString).BeginTransection();
        }

        public void Dispose()
        {
            new ScoopDatabase<dynamic>(ConnectionString).Dispose();
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
    }

}