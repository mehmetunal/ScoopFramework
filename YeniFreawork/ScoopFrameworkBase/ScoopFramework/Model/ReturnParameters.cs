using System.Collections.Generic;
using System.Data.SqlClient;

namespace ScoopFramework.Model
{
    public class ReturnParameters
    {
        public ReturnParameters()
        {
            Parameters = new List<SqlParameter>();
        }
        public List<SqlParameter> Parameters { get; set; }
        public string paramKey { get; set; }
        public string paramValue { get; set; }
        public string updateParameters { get; set; }
    }
}
