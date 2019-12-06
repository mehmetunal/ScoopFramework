using System.Web.Mvc;

namespace ScoopFramework.Attribute
{
    public class AllowEveryone : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            return;
        }

    }
}
