using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ScoopFramework.Themes;

namespace KodTest
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //TODO:TEMA
            //EngineContext.Initialize(false);
            //remove all view engines
            //ViewEngines.Engines.Clear();
            ////except the themeable razor view engine we use
            //ViewEngines.Engines.Add(new ThemeableRazorViewEngine());

        }
    }
}
