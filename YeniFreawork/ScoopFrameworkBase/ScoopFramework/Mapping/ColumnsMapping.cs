using System;

namespace ScoopFramework.Mapping
{
    public class ColumnsMapping
    {
        public Type Type { get; set; }
        public string Name { get; set; }
    }

    public class ScoopPagging
    {
        public int Page { get; set; }
        public int PageCount { get; set; }
    }
}