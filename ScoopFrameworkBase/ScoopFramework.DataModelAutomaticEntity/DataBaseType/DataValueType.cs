namespace ScoopFramework.DataModelAutomaticEntity.DataBaseType
{
    public class DataValueType
    {
        public string ColumnTypes(string type)
        {
            string result;
            switch (type)
            {
                case "int":
                case "smallint":
                    result = "int";
                    break;
                case "money":
                    result = "decimal";
                    break;
                case "real":
                    result = "double";
                    break;
                case "bit":
                    result = "bool";
                    break;
                case "varchar":
                case "nchar":
                case "ntext":
                case "nvarchar":
                    result = "string";
                    break;
                case "smalldatetime":
                case "datetime":
                    result = "DateTime";
                    break;
                default:
                    result = "object";
                    break;
            }
            return result;
        }
    }
}
