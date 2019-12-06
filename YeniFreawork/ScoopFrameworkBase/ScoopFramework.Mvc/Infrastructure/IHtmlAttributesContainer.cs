using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoopFramework.Mvc.Infrastructure
{
    public interface IHtmlAttributesContainer
    {
        /// <summary>
        /// The HtmlAttributes applied to objects which can have child items
        /// </summary>
        IDictionary<string, object> HtmlAttributes
        {
            get;
        }
    }
}
