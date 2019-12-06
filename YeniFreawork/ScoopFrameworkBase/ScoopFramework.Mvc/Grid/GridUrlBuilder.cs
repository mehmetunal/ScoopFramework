using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using ScoopFramework.Mvc.Extensions;
using ScoopFramework.Mvc.Grid.Settings;

namespace ScoopFramework.Mvc.Grid
{
    public class GridUrlBuilder : IGridUrlBuilder
    {
        private readonly IGrid grid;


        public GridUrlBuilder(IGrid grid)
        {
            this.grid = grid;
        }

        

     
        public string SelectUrl()
        {
            return Url(grid.DataSource.Transport.Read);
        }

        public string SelectUrl(string key, object value)
        {
            throw new NotImplementedException();
        }


        public string SelectUrl(object dataItem)
        {

            var navigatable = grid.DataSource.Transport.Read;

            var selectRouteValues = PrepareRouteValues(navigatable.RouteValues);


            return navigatable.GenerateUrl(grid.ViewContext, grid.UrlGenerator, selectRouteValues);
        }

        public string CancelUrl(object dataItem)
        {
            throw new NotImplementedException();
        }

        public string EditUrl(object dataItem)
        {
            throw new NotImplementedException();
        }

        public string AddUrl(object dataItem)
        {
            throw new NotImplementedException();
        }

        public string InsertUrl(object dataItem)
        {
            throw new NotImplementedException();
        }

        public string UpdateUrl(object dataItem)
        {
            throw new NotImplementedException();
        }

        public string DeleteUrl(object dataItem)
        {
            throw new NotImplementedException();
        }


        public string Url(INavigatable navigatable, Action<RouteValueDictionary> configurator)
        {
            RouteValueDictionary routeValues = PrepareRouteValues(navigatable.RouteValues);
            configurator(routeValues);

            return navigatable.GenerateUrl(grid.ViewContext, grid.UrlGenerator, routeValues);
        }

        public string Url(INavigatable navigatable)
        {
            return Url(navigatable, true);
        }

        public string Url(INavigatable navigatable, bool copy)
        {
            var routeValues = new RouteValueDictionary(navigatable.RouteValues);

            if (copy)
            {
                routeValues = PrepareRouteValues(navigatable.RouteValues);
            }

            return navigatable.GenerateUrl(grid.ViewContext, grid.UrlGenerator, routeValues);
        }

        public RouteValueDictionary PrepareRouteValues(RouteValueDictionary routeValues)
        {
            RouteValueDictionary result = new RouteValueDictionary(routeValues);

            result.Merge(grid.ViewContext.RouteData.Values, false);


            foreach (string key in grid.ViewContext.HttpContext.Request.QueryString)
            {
                if (key != null && key != "X-Requested-With" && !result.ContainsKey(key))
                {
                    result[key] = grid.ViewContext.HttpContext.Request.QueryString[key];
                }
            }

            return result;
        }

    }
}
