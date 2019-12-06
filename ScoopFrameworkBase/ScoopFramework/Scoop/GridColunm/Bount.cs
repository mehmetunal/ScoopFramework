using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ScoopFramework.Scoop.GridColunm
{
    public class Bount
    {
        public Expression<Func<object>> Property { get; set; }
        public string Title { get; set; }
        public int Width { get; set; }
        public bool Sortable { get; set; }
        public bool Filtre { get; set; }
        public string Class { get; set; }
    }
}
