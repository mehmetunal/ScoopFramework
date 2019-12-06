using System.Collections.Generic;

namespace ScoopFramework.Mvc
{
    public class Transport : JsonObject
    {
        public Transport()
        {
            Read = new CrudOperation();
        }

        public string Prefix { get; set; }


        public CrudOperation Read { get; private set; }

        protected override void Serialize(IDictionary<string, object> json)
        {
            throw new System.NotImplementedException();
        }
    }
}