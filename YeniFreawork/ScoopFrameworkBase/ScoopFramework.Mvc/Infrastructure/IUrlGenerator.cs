using System.Diagnostics.CodeAnalysis;
using System.Web.Routing;

namespace ScoopFramework.Mvc.Infrastructure
{
    public interface IUrlGenerator
    {
        string Generate(RequestContext requestContext, INavigatable navigationItem);
        string Generate(RequestContext requestContext, INavigatable navigationItem, RouteValueDictionary routeValues);

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "Should accept url as string.")]
        string Generate(RequestContext requestContext, string url);
    }
}
