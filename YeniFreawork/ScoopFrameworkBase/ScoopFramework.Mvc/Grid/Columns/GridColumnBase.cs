using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScoopFramework.Mvc.Grid.Settings;

namespace ScoopFramework.Mvc
{
    /// <summary>
    /// The base class for all columns in Kendo Grid for ASP.NET MVC.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GridColumnBase<T> : JsonObject, IGridColumn where T : class
    {
        public string Format
        {
            get
            {
                return Settings.Format;
            }
            set
            {
                Settings.Format = value;
            }
        }

     
        protected GridColumnBase(Grid<T> grid)
        {
            Grid = grid;
            Settings = new GridColumnSettings();
            Visible = true;
        }

        /// <summary>
        /// Gets or sets the grid.
        /// </summary>
        /// <value>The grid.</value>
        public Grid<T> Grid
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the member of the column.
        /// </summary>
        /// <value>The member.</value>
        public string Member
        {
            get
            {
                return Settings.Member;
            }

            set
            {
                Settings.Member = value;
            }
        }

        /// <summary>
        /// Gets the template of the column.
        /// </summary>
        public virtual Action<T> Template
        {
            get;
            set;
        }

        protected override void Serialize(IDictionary<string, object> json)
        {
            if (Title.HasValue())
            {
                json["title"] = Title;
            }

            if (HtmlAttributes.Any())
            {
                var attributes = new Dictionary<string, object>();

                HtmlAttributes.Each(attr => {
                    attributes[HttpUtility.HtmlAttributeEncode(attr.Key)] = HttpUtility.HtmlAttributeEncode(attr.Value.ToString());
                });

                json["attributes"] = attributes;
            }


            if (Hidden)
            {
                json["hidden"] = true;
            }

          

            if (Width.HasValue())
            {
                json["width"] = Width;
            }

            if (ClientTemplate.HasValue())
            {
                json["template"] = HttpUtility.UrlDecode(ClientTemplate);
            }

            if (ClientFooterTemplate.HasValue())
            {
                json["footerTemplate"] = HttpUtility.UrlDecode(ClientFooterTemplate);
            }

         
        }

        /// <summary>
        /// Gets the header template of the column.
        /// </summary>
        public HtmlTemplate HeaderTemplate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the footer template of the column.
        /// </summary>
       

        public virtual Func<T, object> InlineTemplate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title of the column.
        /// </summary>
        /// <value>The title.</value>
        public virtual string Title
        {
            get
            {
                return Settings.Title;
            }
            set
            {
                Settings.Title = value;
            }
        }

        /// <summary>
        /// Gets or sets the width of the column.
        /// </summary>
        /// <value>The width.</value>
        public string Width
        {
            get
            {
                return Settings.Width;
            }
            set
            {
                Settings.Width = value;
            }
        }

        public string ClientTemplate
        {
            get
            {
                return Settings.ClientTemplate;
            }
            set
            {
                Settings.ClientTemplate = value;
            }
        }

        public string ClientFooterTemplate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this column is hidden.
        /// </summary>
        /// <value><c>true</c> if hidden; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// Hidden columns are output as HTML but are not visible by the end-user.
        /// </remarks>
        public virtual bool Hidden
        {
            get
            {
                return Settings.Hidden;
            }
            set
            {
                Settings.Hidden = value;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether this column is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>. The default value is <c>true</c>.</value>
        /// <remarks>
        /// Invisible columns are not output in the HTML.
        /// </remarks>
        public bool Visible
        {
            get
            {
                return Settings.Visible;
            }
            set
            {
                Settings.Visible = value;
            }
        }

        /// <summary>
        /// Gets the HTML attributes of the cell rendered for the column
        /// </summary>
        /// <value>The HTML attributes.</value>
        public IDictionary<string, object> HtmlAttributes
        {
            get
            {
                return Settings.HtmlAttributes;
            }
        }

        IGrid IGridColumn.Grid
        {
            get
            {
                return Grid;
            }
        }

      

        internal GridColumnSettings Settings
        {
            get;
            set;
        }

       
    }
}