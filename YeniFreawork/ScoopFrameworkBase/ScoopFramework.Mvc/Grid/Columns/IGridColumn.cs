using System.Collections.Generic;

namespace ScoopFramework.Mvc
{
    public interface IGridColumn
    {
        string ClientTemplate
        {
            get;
            set;
        }
        IDictionary<string, object> HtmlAttributes
        {
            get;
        }

        string Title
        {
            get;
            set;
        }

        bool Visible
        {
            get;
            set;
        }

        string Width
        {
            get;
            set;
        }

        IGrid Grid
        {
            get;
        }

    }
}