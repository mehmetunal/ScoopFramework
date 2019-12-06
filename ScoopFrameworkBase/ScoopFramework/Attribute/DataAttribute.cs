using System;

namespace ScoopFramework.Attribute
{
    public class StartDateAttribute : System.Attribute { }
    public class EndDateAttribute : System.Attribute { }
    public class ImageTypeAttribute : System.Attribute { }
    public class DbFormulAttribute : System.Attribute { }
    public class UserCreatedAttribute : System.Attribute { }
    public class EtiketAttribute : System.Attribute { }
    public class UserUpdateAttribute : System.Attribute { }
    public class DateUpdateAttribute : System.Attribute { }

    public class ForingnKeyTable : System.Attribute
    {
        public ForingnKeyTable(string tableName)
        {
            _tableName = tableName;
        }
        public string tableName { get { return _tableName; } }
        private string _tableName;
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class Replace : System.Attribute
    {
        public Replace(string nameParam)
        {
            _name = nameParam;
        }
        public string name { get { return _name; } }
        private string _name;
    }
    public class RealName : System.Attribute
    {
        public RealName(string nameParam)
        {
            _name = nameParam;
        }
        public string name { get { return _name; } }
        private string _name;

    }
    public class SeoAttribute : System.Attribute { }
    public class EtiketSeoAttribute : System.Attribute { }

    public class ProcessedSeoName : System.Attribute { }
    public class DataControlColumn : System.Attribute { }
}
