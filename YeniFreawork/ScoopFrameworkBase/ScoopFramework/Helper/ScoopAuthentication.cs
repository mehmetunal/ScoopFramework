using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ScoopFramework.Helper
{
    public enum ScoopRole
    {
        //Yönetici
        Manager = 0,
        //Kullanıcı
        Employee = 1,
        //Misafir Kullanıcı
        Guest = 2
    }
    public class ScoopAuthenticationAttribute : ActionFilterAttribute
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }
        public string SessionName { get; set; }
        public ScoopRole Role { get; set; }
        public ScoopAuthenticationAttribute(string action, string controller, string area, string sessionName, ScoopRole role = ScoopRole.Manager)
        {
            Action = action;
            Controller = controller;
            Area = area;
            SessionName = sessionName;
            Role = role;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session[SessionName] == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                    {
                        {"Action", Action},
                        {"controller", Controller},
                        {"area", Area},
                        {"returnUrl", filterContext.HttpContext.Request.RawUrl}
                    });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
