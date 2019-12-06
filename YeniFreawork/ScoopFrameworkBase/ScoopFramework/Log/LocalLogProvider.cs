using System;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Configuration;
using ScoopFramework.Entity;
using ScoopFramework.Enums;
using ScoopFramework.Helper;
using ScoopFramework.Mapping;

namespace ScoopFramework.Log
{
    /// <summary>
    /// Class for logging error messages.
    /// </summary>
    public class LocalLogProvider : ILogProvider
    {
        #region Member
        /// <summary>
        /// The physical path to the log folder
        /// </summary>
        private readonly string path;

        /// <summary>
        /// The physical path to the log file
        /// </summary>
        private readonly string file;

        /// <summary>
        /// Mutex for keeping it thread safe.
        /// </summary>
        private readonly object mutex = new object();
        #endregion

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LocalLogProvider()
            : this(HttpContext.Current.Server.MapPath("~/App_Data/Logs"), HttpContext.Current.Server.MapPath(string.Format("~/App_Data/Logs/Log#{0}.txt", DateTime.Now.ToString("yyyy-MM-dd"))))
        {

        }

        /// <summary>
        /// Creates a new local log provider using the specified folder and file for output
        /// </summary>
        /// <param name="folderPath">The folder containing the log file</param>
        /// <param name="filePath">The file path to the log file</param>
        public LocalLogProvider(string folderPath, string filePath)
        {

            if (string.IsNullOrEmpty(folderPath)) throw new ArgumentNullException("folderPath");
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException("filePath");

            this.path = folderPath;
            this.file = filePath;

            // Create the log directory if it doesn't exist
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Logs an error to the log file.
        /// </summary>
        /// <param name="origin">The origin of the error</param>
        /// <param name="message">The message</param>
        /// <param name="details">Optional error details</param>
        public void Error(string origin, string message, System.Exception details = null)
        {
            lock (mutex)
            {
                using (var writer = new StreamWriter(file, true))
                {
                    writer.WriteLine(String.Format(
                        "ERROR [{0}] Origin [{1}] Message [{2}]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), origin, message));
                    if (details != null)
                        writer.WriteLine(details);
                }
            }
        }
        public void Message(string url, Guid createdby, EnumTraceLogProviderMethod method, EnumTraceLogProviderDurum durum, string message, string messageDetail, Guid? logTableId = null)
        {
            var logTableSuccess = WebConfigurationManager.AppSettings["LogTable"];
            if (string.IsNullOrEmpty(logTableSuccess))
            {
                const string logSqlQuery = @"CREATE TABLE [dbo].[SCP_Log](
	                    [id] [uniqueidentifier] NOT NULL,
	                    [createddate] [datetime] NULL,
                        [createdby] [uniqueidentifier] NOT NULL,
	                    [Url] [nvarchar](500) NULL,
	                    [Method] [int] NULL,
	                    [Baslik] [nvarchar](250) NULL,
	                    [Mesaj] [nvarchar](500) NULL,
	                    [Detay] [nvarchar](max) NULL,
                        [LogTableId] [uniqueidentifier] NOT NULL,
                        [Durum] [int] NULL,
                     CONSTRAINT [PK_SYS_Log] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

                    GO
                    ALTER TABLE [dbo].[SCP_Log] ADD  CONSTRAINT [DF_SYS_Log_id]  DEFAULT (newid()) FOR [id]
                    GO
                    ALTER TABLE [dbo].[SCP_Log] ADD  CONSTRAINT [DF_SYS_Log_createddate]  DEFAULT (getdate()) FOR [createddate]
                    GO";
                var connection = new SqlConnection(WebConfigurationManager.AppSettings["DBConnection"]);
                var command = new SqlCommand("SELECT COUNT(TABLE_NAME) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='SCP_Log'");
                command.Connection = connection;
                command.Connection.Open();
                try
                {
                    var rs = (int)command.ExecuteScalar();
                    if (rs > 0)
                    {
                        command.CommandText = logSqlQuery;
                        command.ExecuteNonQuery();
                    }
                }
                catch (System.Exception ex)
                {
                    Error(ex.Message, "LOG TABLE");
                    command.Connection.Close();
                    command.Connection.Dispose();
                }
                finally
                {
                    command.Connection.Close();
                    command.Connection.Dispose();
                }
            }

            logTableId = logTableId.HasValue == true ? logTableId : Guid.NewGuid();
            var db = new Database<SCP_Log>().Insert(new SCP_Log()
            {
                id = Guid.NewGuid(),
                Url = url,
                Baslik = string.Format("{0} İşlemi yapıldı.", method.ToDescription()),
                createddate = DateTime.Now,
                createdby = createdby,
                Mesaj = message,
                Detay = messageDetail,
                LogTableId = logTableId.Value,
                Method = (int)method,
                Durum = (int)durum
            });
        }
    }
}
