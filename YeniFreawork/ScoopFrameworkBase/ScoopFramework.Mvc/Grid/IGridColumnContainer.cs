using System.Collections.Generic;

namespace ScoopFramework.Mvc
{
    public interface IGridColumnContainer<T> where T : class
    {
        IList<GridColumnBase<T>> Columns
        {
            get;
        }
    }
}