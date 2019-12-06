using System.Web.Mvc;
using ScoopFramework.Mvc.Infrastructure;

namespace ScoopFramework.Mvc
{
    public interface IWidget : IHtmlAttributesContainer
    {
        string Id { get; }

        string Name { get; }

        ModelMetadata ModelMetadata { get; }

        ViewContext ViewContext { get; }

        ViewDataDictionary ViewData { get; }
    }
}