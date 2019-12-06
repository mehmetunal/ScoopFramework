using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using ScoopFramework.Mvc.Extensions;
using ScoopFramework.Mvc.UI.Fluent;

namespace ScoopFramework.Mvc
{
    public class DataSource : JsonObject
    {
        public DataSource()
        {
            Transport = new Transport();
        }
        public Transport Transport
        {
            get;
            private set;
        }

        protected override void Serialize(IDictionary<string, object> json)
        {
            throw new NotImplementedException();
        }
    }
}
