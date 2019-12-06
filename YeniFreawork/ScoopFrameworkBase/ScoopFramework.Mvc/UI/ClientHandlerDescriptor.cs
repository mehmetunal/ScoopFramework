using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoopFramework.Mvc
{
    public class ClientHandlerDescriptor
    {
        /// <summary>
        /// A Razor template delegate.
        /// </summary>
        public Func<object, object> TemplateDelegate { get; set; }

        /// <summary>
        /// The name of the JavaScript function which will be called as a handler.
        /// </summary>
        public string HandlerName { get; set; }

        public bool HasValue()
        {
            return HandlerName.HasValue() || TemplateDelegate != null;
        }
    }
}
