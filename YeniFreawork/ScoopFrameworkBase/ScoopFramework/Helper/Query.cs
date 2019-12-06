namespace ScoopFramework.Helper
{
    public class Query
    {
        public string Command { get; set; }
        public QueryParameter[] Parameters { get; set; }
        public bool IsStoredProcedure { get; set; }

        public Query()
        {
            Parameters = new QueryParameter[0];
        }
    }
    public class QueryParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Name, Value);
        }
    }


}
