using System;
using System.Linq.Expressions;
using ScoopFramework.DataTables;
using ScoopFramework.Mapping;

namespace ScoopFramework.Linq
{
    public enum Component { Where, From, SubCube, CreatedSet, CreatedMember, OrderBy, OrderByDesc, Paggin, LikeLeft, LikeIn, LikeRight,NotIn,In, Between, Filtre, DataTablesRequest,Count }

    public class ScoopComponent
    {
        public Component ComponentType { get; set; }
        internal Expression Creator { get; set; }
        internal object Entity { get; set; }
        internal IDataTablesRequest DataTablesRequest { get; set; }
        internal ScoopPagging ScoopPagging { get; set; }
        public byte DeclarationOrder { get; set; }
        public string Name { get; set; }
        public byte? Axis { get; set; }

        public ScoopComponent(Component componentType)
            : this(componentType, null, null) { }

        public ScoopComponent(Component componentType, string name)
            : this(componentType, name, null) { }

        public ScoopComponent(Component componentType, string name, Expression componentAssembler, object entity = null, IDataTablesRequest dataTablesRequest = null)
        {
            ComponentType = componentType;
            Creator = componentAssembler;
            Entity = entity;
            DataTablesRequest = dataTablesRequest;
            Name = name;
        }

        public ScoopComponent AssembleComponent<T>(Expression<Func<T, object>> componentAssembler, object entity = null, IDataTablesRequest dataTablesRequest = null)
        {
            Creator = componentAssembler;
            Entity = entity;
            DataTablesRequest = dataTablesRequest;
            return this;
        }
    }
}
