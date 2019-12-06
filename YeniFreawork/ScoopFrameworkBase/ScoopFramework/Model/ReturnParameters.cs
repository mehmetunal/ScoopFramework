using System.Collections.Generic;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace ScoopFramework.Model
{
    public class ReturnParameters
    {
        public ReturnParameters()
        {
            Parameters = new List<SqlParameter>();
            MySqlParameter = new List<MySqlParameter>();
        }
        public List<SqlParameter> Parameters { get; set; }
        public List<MySqlParameter> MySqlParameter { get; set; }
        public string paramKey { get; set; }
        public string paramValue { get; set; }
        public string updateParameters { get; set; }
    }
}
