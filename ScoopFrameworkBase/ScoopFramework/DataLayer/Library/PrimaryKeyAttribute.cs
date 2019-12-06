using System;

namespace ScoopFramework.DataLayer.Library
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class PrimaryKeyAttribute : System.Attribute
    {

    }
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class ForingnKeyAttribute : System.Attribute
    {

    }
}
