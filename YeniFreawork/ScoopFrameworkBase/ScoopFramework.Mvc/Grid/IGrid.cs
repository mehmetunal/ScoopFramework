using System.Collections.Generic;
using System.Web.Mvc;
using ScoopFramework.Mvc.Infrastructure;

namespace ScoopFramework.Mvc
{
    public interface IGrid
    {
        DataSource DataSource
        {
            get;
        }
        IUrlGenerator UrlGenerator
        {
            get;
        }

        ViewContext ViewContext
        {
            get;
        }

        IEnumerable<IGridColumn> Columns
        {
            get;
        }

        bool IsInClientTemplate
        {
            get;
        }
    }
}