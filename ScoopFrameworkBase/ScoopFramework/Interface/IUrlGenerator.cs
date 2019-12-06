using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace ScoopFramework.Interface
{
    public interface IUrlGenerator
    {
        string Generate(RequestContext requestContext, INavigatable navigationItem);
        string Generate(RequestContext requestContext, string url);
        string Generate(RequestContext requestContext, INavigatable navigationItem, RouteValueDictionary routeValues);
    }
}
