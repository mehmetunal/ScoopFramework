using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace ScoopFramework.Scoop
{
    public static class ScoopControl
    {
        /*Sınırsız Colunm Yani property Almalı*/

        public static MvcHtmlString ScoopGridFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {

            return MvcHtmlString.Empty;
        }
        public static MvcHtmlString ScoopGridFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {

            return MvcHtmlString.Empty;
        }
        public static MvcHtmlString SccopDropListDownFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            return MvcHtmlString.Empty;
        }
    }
}
