using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ScoopFramework.Expressions
{
    internal abstract class ExpressionBuilderBase
    {
        private readonly ExpressionBuilderOptions options;
        private readonly Type itemType;
        private ParameterExpression parameterExpression;

        protected ExpressionBuilderBase(Type itemType)
        {
            this.itemType = itemType;
            this.options = new ExpressionBuilderOptions();
        }

        public ExpressionBuilderOptions Options
        {
            get
            {
                return this.options;
            }
        }

        protected internal Type ItemType
        {
            get
            {
                return this.itemType;
            }
        }

        protected internal ParameterExpression ParameterExpression
        {
            get
            {
                if (this.parameterExpression == null)
                {
                    this.parameterExpression = Expression.Parameter(this.ItemType, "item");
                }

                return this.parameterExpression;
            }
            set
            {
                this.parameterExpression = value;
            }
        }
    }
}
