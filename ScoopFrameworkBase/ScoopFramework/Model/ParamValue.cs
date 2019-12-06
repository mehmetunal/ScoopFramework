using System.Collections.Generic;
using System.Web;
namespace ScoopFramework.Model
{
    /// <summary>
    /// The param value.
    /// </summary>
    public class ParamValue
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public object Value { get; set; }
        public HttpPostedFileBase ImageValue { get; set; }
        /// <summary>
        /// Gets or sets the where.
        /// </summary>
        public Dictionary<string, string> Where { get; set; }
        /// <summary>
        /// Gets or sets the between.
        /// </summary>
        public string Between { get; set; }
        /// <summary>
        /// Gets or sets the order by.
        /// </summary>
        public string OrderByAsc { get; set; }
        /// <summary>
        /// Gets or sets the order by descending.
        /// </summary>
        public string OrderByDescending { get; set; }
        public Paging page { get; set; }
        public Dictionary<string, string> Like { get; set; }
    }
}
