using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using ScoopFramework.Attribute;
using ScoopFramework.Extension;
using ScoopFramework.UserLogin;

namespace ScoopFramework.UserRole
{
    public sealed class UserRolGuidControlAuthorize : AuthorizeAttribute
    {

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowEveryone), true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowEveryone), true);
            if (!skipAuthorization)
            {
                var usersucces = UserRoleControl(filterContext);
                if (usersucces)
                {
                    var succes = GuidControl(filterContext);
                    if (!succes)
                    {
                        SayfaBadIdYönlendirme(filterContext);
                    }
                }
                else
                {
                    SayfaRolYönlendirme(filterContext);
                }
            }
        }

        private void SayfaRolYönlendirme(AuthorizationContext filterContext)
        {
            filterContext.Result =
                new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"action", filterContext.HttpContext.Request["action"]},
                    {"controller", filterContext.HttpContext.Request["controller"]},
                    {"area", string.Empty},
                    {"returnUrl", filterContext.HttpContext.Request.RawUrl}
                });
        }
        private void SayfaBadIdYönlendirme(AuthorizationContext filterContext)
        {

            FeedbackExcetpion.Feedback feedBack = new FeedbackExcetpion.Feedback();
            feedBack.Error("Geçersiz id girdiniz.");
            filterContext.Result =
               new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"action", "Index"},
                    {"controller", filterContext.RouteData.Values["controller"].ToString()},
                  });
        }


        public bool UserRoleControl(AuthorizationContext filterContext)
        {
            //if (filterContext.RouteData.Values["controller"] == null)
            //{
            //    return false;
            //}

            //var controller = filterContext.RouteData.Values["controller"].ToString();

            //var action = filterContext.RouteData.Values["action"].ToString();

            //var requestPage = string.Format("{0}/{1}", controller, action);

            #region rolü olmayan sayfala içi
            //if (!ActionControl(requestPage, filterContext) || StaticTempMethod.CustomAttr.Any(pair => pair.Key.Equals(requestPage)))
            //{
            //    return false;
            //}
            #endregion


            if (!SessionTicketNullControl(filterContext))
            {
                return false;
            }
            return true;
        }
        private bool GuidControl(AuthorizationContext filterContext)
        {
            if (filterContext.RouteData.Values["id"] != null || filterContext.RequestContext.HttpContext.Request["id"] != null || filterContext.RouteData.Values["Id"] != null || filterContext.RequestContext.HttpContext.Request["Id"] != null)
            {
                var rountDataId = filterContext.RouteData.Values["id"] ?? filterContext.RouteData.Values["Id"];
                var requestId = filterContext.RequestContext.HttpContext.Request["id"] ?? filterContext.RequestContext.HttpContext.Request["Id"];

                if (rountDataId != null)
                {
                    var rountId = rountDataId.ToString();

                    if (!string.IsNullOrEmpty(rountId))
                    {
                        if (!rountId.IsValidGuid())
                        {
                            if (!rountId.IsNumeric())
                            {
                                return false;
                            }
                        }
                    }
                }

                var reqId = requestId;

                if (!string.IsNullOrEmpty(reqId))
                {
                    if (!reqId.IsValidGuid())
                    {
                        if (!reqId.IsNumeric())
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool SessionTicketNullControl(AuthorizationContext filterContext)
        {

            if (filterContext.RequestContext.HttpContext.Session != null &&
                !SecurityControl.GetSecurityControl(filterContext.RequestContext.HttpContext.Session["userStatus"]))
            {
                return false;
            }
            return true;
        }

        //private bool ActionControl(string requestPage, AuthorizationContext filterContext)
        //{
        //    if (filterContext.RequestContext.HttpContext.Session != null)
        //    {
        //        var userStatus = (PageSecurity)filterContext.RequestContext.HttpContext.Session["userStatus"];
        //        if (userStatus != null)
        //        {
        //            if (userStatus.AuthorityActionRoles != null)
        //            {
        //                var actionControl =
        //                    userStatus.AuthorityActionRoles.FirstOrDefault(role => role.Action.ToUpperInvariant().Equals(requestPage.ToUpperInvariant()));
        //                //userStatus.AuthorityActionRoles.FirstOrDefault(role => role.Action.ToUpperInvariant().Equals(requestPage.ToUpperInvariant()));

        //                if (actionControl == null || (bool)!actionControl.Status)
        //                {
        //                    return false;
        //                }
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    return true;
        //}
    }
}