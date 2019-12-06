using System.Web.Routing;

namespace ScoopFramework.Mvc.Infrastructure
{
    public interface INavigationItemAuthorization
    {
        bool IsAccessibleToUser(RequestContext requestContext, INavigatable navigationItem);
    }
}