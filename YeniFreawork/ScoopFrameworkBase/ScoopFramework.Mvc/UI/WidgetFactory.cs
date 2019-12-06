using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ScoopFramework.Mvc.Extensions;
using ScoopFramework.Mvc.Infrastructure;

namespace ScoopFramework.Mvc
{
    public class WidgetFactory : IHideObjectMembers
    {
        public WidgetFactory(HtmlHelper htmlHelper)
        {
            HtmlHelper = htmlHelper;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IJavaScriptInitializer Initializer
        {
            get;
            private set;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IUrlGenerator UrlGenerator
        {
            get;
            private set;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public HtmlHelper HtmlHelper
        {
            get;
            set;
        }

        private ViewContext ViewContext
        {
            get
            {
                return HtmlHelper.ViewContext;
            }
        }

        private ViewDataDictionary ViewData
        {
            get
            {
                return HtmlHelper.ViewData;
            }
        }

        /// <summary>
        /// Creates a new <see cref="Mvc.Grid{T}"/> bound to the specified data item type.
        /// </summary>
        /// <example>
        /// <typeparam name="T">The type of the data item</typeparam>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().Grid&lt;Order&gt;()
        ///             .Name("Grid")
        ///             .BindTo(Model)
        /// %&gt;
        /// </code>
        /// </example>      
        public virtual GridBuilder<T> Grid<T>() where T : class
        {
            return new GridBuilder<T>(new Grid<T>(ViewContext, Initializer, UrlGenerator));
        }

        /// <summary>
        /// Creates a new <see cref="Mvc.Grid{T}"/> bound to the specified data source.
        /// </summary>
        /// <typeparam name="T">The type of the data item</typeparam>
        /// <param name="dataSource">The data source.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().Grid(Model)
        ///             .Name("Grid")
        /// %&gt;
        /// </code>
        /// </example>


        /// <summary>
        /// Creates a new <see cref="Mvc.Grid{T}"/> bound to a DataTable.
        /// </summary>
        /// <param name="dataSource">DataTable from which the grid instance will be bound</param>


        /// <summary>
        /// Creates a new <see cref="Mvc.Grid{T}"/> bound to a DataView.
        /// </summary>
        /// <param name="dataSource">DataView from which the grid instance will be bound</param>

        /// <summary>
        /// Creates a new <see cref="Mvc.Grid{T}"/> bound an item in ViewData.
        /// </summary>
        /// <typeparam name="T">Type of the data item</typeparam>
        /// <param name="dataSourceViewDataKey">The data source view data key.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().Grid&lt;Order&gt;("orders")
        ///             .Name("Grid")
        /// %&gt;
        /// </code>
        /// </example>
    }

    public class WidgetFactory<TModel> : WidgetFactory
    {
        private readonly string minimumValidator;
        private readonly string maximumValidator;

        public WidgetFactory(HtmlHelper<TModel> htmlHelper)
            : base(htmlHelper)
        {
            this.HtmlHelper = htmlHelper;


            minimumValidator = "min";
            maximumValidator = "max";
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new HtmlHelper<TModel> HtmlHelper
        {
            get;
            set;
        }

        private string GetName(LambdaExpression expression)
        {
            return HtmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
        }

        private string GetValue<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            object model = ModelMetadata.FromLambdaExpression(expression, HtmlHelper.ViewData).Model;
            return model != null && model.GetType().IsPredefinedType() ? Convert.ToString(model) : string.Empty;
        }

        private Nullable<TValue> GetRangeValidationParameter<TValue>(IEnumerable<ModelValidator> validators, string parameter) where TValue : struct
        {
            var rangeValidators = validators.OfType<RangeAttributeAdapter>().Cast<RangeAttributeAdapter>();

            object value = null;

            if (rangeValidators.Any())
            {
                var clientValidationsRules = rangeValidators.First()
                                                            .GetClientValidationRules()
                                                            .OfType<ModelClientValidationRangeRule>()
                                                            .Cast<ModelClientValidationRangeRule>();

                if (clientValidationsRules.Any() && clientValidationsRules.First().ValidationParameters.TryGetValue(parameter, out value))
                {
                    return (TValue)Convert.ChangeType(value, typeof(TValue));
                }
            }
            return null;
        }
    }
}
