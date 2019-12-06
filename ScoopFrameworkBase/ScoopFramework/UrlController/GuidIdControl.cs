using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using ScoopFramework.Extension;

namespace ScoopFramework.UrlController
{
    public class GuidIdControl : ActionFilterAttribute
    {
        public string ActionName { get; set; }
        public string RouteValues { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (filterContext.ActionParameters.Count < 0 || filterContext.ActionParameters.Values.FirstOrDefault() == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", ActionName }, { "controller", RouteValues } });
                return;
            }

            var stateParam = filterContext.ActionParameters.FirstOrDefault(p => p.Key == "id");

            if (string.IsNullOrEmpty(stateParam.Key) || stateParam.Value == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", ActionName }, { "controller", RouteValues } });
                return;
            }

            if (string.IsNullOrEmpty(stateParam.Value.ToString()) || !stateParam.Value.ToString().IsValidGuid())
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", ActionName }, { "controller", RouteValues } });
            }

        }
    }
}
