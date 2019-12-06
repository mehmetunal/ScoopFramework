using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ScoopFramework.Mvc.Infrastructure;
using ScoopFramework.Mvc.UI.Fluent;

namespace ScoopFramework.Mvc
{
    public class ServerDataSourceBuilder<TModel> : IHideObjectMembers
         where TModel : class
    {
        private readonly DataSource dataSource;
        private readonly ViewContext viewContext;

        public ServerDataSourceBuilder(DataSource dataSource, ViewContext viewContext)
        {
            this.viewContext = viewContext;
            this.dataSource = dataSource;
        }

        /// <summary>
        /// Configures the URL for Read operation.
        /// </summary> 
        public ServerDataSourceBuilder<TModel> Read(Action<ServerCrudOperationBuilder> configurator)
        {
            configurator(new ServerCrudOperationBuilder(dataSource.Transport.Read, viewContext));

            return this;
        }

        /// <summary>
        /// Sets controller and action for Read operation.
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller Name</param>    
        /// <param name="routeValues">Route values</param>
        public ServerDataSourceBuilder<TModel> Read(string actionName, string controllerName, object routeValues)
        {
            SetOperationUrl(dataSource.Transport.Read, actionName, controllerName, routeValues);

            return this;
        }

        /// <summary>
        /// Sets controller, action and routeValues for Read operation.
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller Name</param>                
        public ServerDataSourceBuilder<TModel> Read(string actionName, string controllerName)
        {
            SetOperationUrl(dataSource.Transport.Read, actionName, controllerName, null);

            return this;
        }
        
        protected virtual void SetOperationUrl(CrudOperation operation, string actionName, string controllerName ,object routeValues)
        {
            operation.Action(actionName, controllerName, routeValues);
            operation.Url = operation.GenerateUrl(viewContext, null);
        }
    }
}
