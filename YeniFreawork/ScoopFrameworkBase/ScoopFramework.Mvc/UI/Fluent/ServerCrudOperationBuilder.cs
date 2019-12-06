using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using ScoopFramework.Mvc.Infrastructure;

namespace ScoopFramework.Mvc.UI.Fluent
{
    public class ServerCrudOperationBuilder : IHideObjectMembers
    {
        private readonly CrudOperation operation;
        private readonly ViewContext viewContext;

        public ServerCrudOperationBuilder(CrudOperation operation, ViewContext viewContext)
        {
            this.viewContext = viewContext;
            this.operation = operation;
        }

        /// <summary>
        /// Sets the route values for the operation.
        /// </summary>
        /// <param name="routeValues">Route values</param>
        public ServerCrudOperationBuilder Route(RouteValueDictionary routeValues)
        {
            operation.Action(routeValues);

            SetUrl();

            return this;
        }

        /// <summary>
        /// Sets the action, contoller and route values for the operation.
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public ServerCrudOperationBuilder Action(string actionName, string controllerName, object routeValues)
        {
            operation.Action(actionName, controllerName, routeValues);

            SetUrl();

            return this;
        }

        /// <summary>
        /// Sets the action, contoller and route values for the operation.
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>        
        public ServerCrudOperationBuilder Action(string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            operation.Action(actionName, controllerName, routeValues);

            SetUrl();

            return this;
        }

        /// <summary>
        /// Sets the action and contoller values for the operation.
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>        
        public ServerCrudOperationBuilder Action(string actionName, string controllerName)
        {
            return Action(actionName, controllerName, (object)null);
        }

        /// <summary>
        /// Sets the route name and values for the operation.
        /// </summary>
        /// <param name="routeName">Route name</param>
        /// <param name="routeValues">Route values</param>        
        public ServerCrudOperationBuilder Route(string routeName, RouteValueDictionary routeValues)
        {
            operation.Route(routeName, routeValues);

            SetUrl();

            return this;
        }

        /// <summary>
        /// Sets the route name and values for the operation.
        /// </summary>
        /// <param name="routeName">Route name</param>
        /// <param name="routeValues">Route values</param>
        public ServerCrudOperationBuilder Route(string routeName, object routeValues)
        {
            operation.Route(routeName, routeValues);

            SetUrl();

            return this;
        }

        /// <summary>
        /// Sets the route name for the operation.
        /// </summary>
        /// <param name="routeName"></param>
        public ServerCrudOperationBuilder Route(string routeName)
        {
            operation.Route(routeName, (object)null);

            SetUrl();

            return this;
        }

        public ServerCrudOperationBuilder Action<TController>(Expression<Action<TController>> controllerAction) where TController : Controller
        {
            operation.Action(controllerAction);

            SetUrl();

            return this;
        }

        private void SetUrl()
        {
            operation.Url = operation.GenerateUrl(viewContext, null);
        }
    }
}
