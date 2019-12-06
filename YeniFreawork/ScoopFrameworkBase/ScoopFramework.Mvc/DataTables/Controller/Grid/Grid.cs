using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using ScoopFramework.Mvc.Extensions;
using ScoopFramework.Mvc.Grid.Settings;
using ScoopFramework.Mvc.Infrastructure;

namespace ScoopFramework.Mvc
{
    public class Grid<T> : WidgetBase, IGridColumnContainer<T>, IGrid where T : class
    {
        private IGridUrlBuilder urlBuilder;
        private string clientRowTemplate;
        public Grid(ViewContext viewContext,
                    IJavaScriptInitializer initializer,
                    IUrlGenerator urlGenerator) : base(viewContext, initializer)
        {

            UrlGenerator = urlGenerator;

            PrefixUrlParameters = true;
            RowTemplate = new HtmlTemplate<T>();
            Columns = new List<GridColumnBase<T>>();
            IsEmpty = true;

            NoRecordsTemplate = new HtmlTemplate();



            DataSource = new DataSource();
           

        }

        public DataSource DataSource
        {
            get;
            private set;
        }

        public string ClientRowTemplate
        {
            get
            {
                return clientRowTemplate;
            }
            set
            {
                clientRowTemplate = HttpUtility.HtmlDecode(value);
            }
        }

      
        /// <summary>
        /// Gets the template which the grid will use to render a row
        /// </summary>
        public HtmlTemplate<T> RowTemplate
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets the scrolling configuration.
        /// </summary>
      
        public IUrlGenerator UrlGenerator
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether custom binding is enabled.
        /// </summary>
        /// <value><c>true</c> if custom binding is enabled; otherwise, <c>false</c>. The default value is <c>false</c></value>
        public bool EnableCustomBinding
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the columns of the grid.
        /// </summary>
        public IList<GridColumnBase<T>> Columns
        {
            get;
            private set;
        }

        IEnumerable<IGridColumn> IGrid.Columns
        {
            get
            {
                return Columns.Cast<IGridColumn>();
            }
        }

        public IList<GridColumnBase<T>> VisibleColumns
        {
            get
            {
                return Columns.Where(c => c.Visible).ToList();
            }
        }

        /// <summary>
        /// Gets the page size of the grid.
        /// </summary>
    

        /// <summary>
        /// Gets or sets a value indicating whether to add the <see cref="WidgetBase.Name"/> property of the grid as a prefix in url parameters.
        /// </summary>
        /// <value><c>true</c> if prefixing is enabled; otherwise, <c>false</c>. The default value is <c>true</c></value>
        public bool PrefixUrlParameters
        {
            get;
            set;
        }

   

        public HtmlTemplate NoRecordsTemplate
        {
            get;
            private set;
        }

        public string Prefix(string parameter)
        {
            return PrefixUrlParameters ? Id + "-" + parameter : parameter;
        }

      

        public bool? AutoBind { get; set; }



     

        public bool AutoGenerateColumns { get; set; }

        public bool IsEmpty
        {
            get;
            set;
        }
 


    }
}
