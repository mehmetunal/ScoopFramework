using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using ScoopFramework.Mvc.UI;

namespace ScoopFramework.Mvc
{
    public static class HtmlHelperExtension
    {
        public static WidgetFactory Scoop(this HtmlHelper helper)
        {
            return new WidgetFactory(helper);
        }

        public static WidgetFactory<TModel> Scoop<TModel>(this HtmlHelper<TModel> helper)
        {
            return new WidgetFactory<TModel>(helper);
        }
    }
}
