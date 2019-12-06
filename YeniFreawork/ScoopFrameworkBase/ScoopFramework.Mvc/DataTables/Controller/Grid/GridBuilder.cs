using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScoopFramework.Mvc.UI.Fluent;

namespace ScoopFramework.Mvc
{
    public class GridBuilder<T> : WidgetBuilderBase<Grid<T>, GridBuilder<T>> where T : class
    {
        public GridBuilder(Grid<T> component) 
            : base(component)
        {

        }
        public GridBuilder<T> DataSource(Action<DataSourceBuilder<T>> configurator)
        {
            configurator(new DataSourceBuilder<T>(Component.DataSource, this.Component.ViewContext));

            return this;
        }
    }
}
