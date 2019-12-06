using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace ScoopFramework.Interface
{
    public interface INavigatable
    {
        // Summary:
        //     Gets or sets the name of the action.
        string ActionName { get; set; }
        //
        // Summary:
        //     Gets or sets the name of the controller.
        string ControllerName { get; set; }
        //
        // Summary:
        //     Gets or sets the name of the route.
        string RouteName { get; set; }
        //
        // Summary:
        //     Gets the route values.
        RouteValueDictionary RouteValues { get; }
        //
        // Summary:
        //     Gets or sets the URL.
        string Url { get; set; }
    }
}
