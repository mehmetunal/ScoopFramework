using System.Linq.Expressions;
namespace ScoopFramework.LinqDynamic
{
    internal class DynamicOrdering
    {
        public Expression Selector;
        public bool Ascending;
    }
}
