using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoopFramework.Mvc
{
    public abstract class JsonObject
    {
        public IDictionary<string, object> ToJson()
        {
            var json = new Dictionary<string, object>();
            return json;
        }
        protected abstract void Serialize(IDictionary<string, object> json);
    }
}