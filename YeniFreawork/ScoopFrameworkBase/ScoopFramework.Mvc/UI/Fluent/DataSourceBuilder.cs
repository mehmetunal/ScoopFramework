using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ScoopFramework.Mvc.Infrastructure;

namespace ScoopFramework.Mvc.UI.Fluent
{
    public class DataSourceBuilder<TModel> : IHideObjectMembers
       where TModel : class
    {
        protected readonly DataSource dataSource;
        protected readonly ViewContext viewContext;

        public DataSourceBuilder(DataSource dataSource, ViewContext viewContext)
        {
            this.viewContext = viewContext;
            this.dataSource = dataSource;
        }

        /// <summary>
        /// Use it to configure Server binding.
        /// </summary>        
        public ServerDataSourceBuilder<TModel> Server()
        {
            return new ServerDataSourceBuilder<TModel>(dataSource, viewContext);
        }
    }
}
