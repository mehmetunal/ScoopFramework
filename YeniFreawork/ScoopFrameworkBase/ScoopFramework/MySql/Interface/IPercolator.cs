using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MySql.Data.MySqlClient;
using ScoopFramework.DataTables;
using ScoopFramework.Exception;
using ScoopFramework.Helper;
using ScoopFramework.Linq;
using ScoopFramework.Mapping;

namespace ScoopFramework.MySql.Interface
{
    internal class Translation
    {
        public byte Type { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public byte? DeclarationOrder { get; set; }
        public bool IsNonEmpty { get; set; }

        public Translation(byte type, string value)
            : this(type, value, false) { }

        public Translation(byte type, string value, bool isNonEmpty)
        {
            Type = type;
            Value = value;
            IsNonEmpty = isNonEmpty;
        }
    }
    internal class IPercolator<T>
    {

        const byte _FROM = 191;
        const byte _WHERE = 192;
        const byte _WMEMBER = 193;
        const byte _WSET = 194;
        const byte _SUBCUBE = 195;
        const byte _ORDERBY = 196;
        const byte _ORDERBYDESC = 197;
        const byte _Paggin = 198;

        const byte _LikeLeft = 199;
        const byte _LikeIn = 201;
        const byte _LikeRight = 202;
        const byte _Between = 203;
        const byte _Filtre = 204;
        const byte _DataTablesRequest = 205;
        const byte _Count = 206;



        byte _setDepth;
        byte _memberDepth;


        List<ScoopComponent> _components;
        List<Translation> _translations;
        public List<MySqlParameter> _parameter;
        byte _currentAxis;
        Component? _currentComponent;
        public string ScoopCommand { get; private set; }

        public IPercolator(List<ScoopComponent> components = null)
        {
            _setDepth = 0;
            _memberDepth = 0;

            _translations = new List<Translation>();
            _components = components;
            _currentComponent = null;
            _parameter = new List<MySqlParameter>();
            ScoopCommand = translate();
        }
        string translate()
        {
            try
            {

                var baseName = new[] { "Object", "BaseEntity" };

                string from = "";

                if (baseName.Contains(typeof(T).BaseType.Name))
                {
                    from = typeof(T).Name;
                }
                else
                {
                    from = typeof(T).BaseType.Name;
                }

                _translations.Add(new Translation(_FROM, string.Format("FROM {0}", from)));
            }
            catch (NullReferenceException e)
            {
                throw new PercolatorException(string.Format("The cube type of '{0}' is not queryable", typeof(T).Name));
            }


            _components.Where(w => w.ComponentType != Component.Paggin).ForEach(component =>
            {
                _currentComponent = component.ComponentType;
                _currentAxis = getComponentValue(component.ComponentType);
                Evaluate(component.Creator, component.Entity, component.DataTablesRequest);
            });

            var scoopPaging = _components.Where(x => x.ScoopPagging != null).Select(x => x.ScoopPagging).FirstOrDefault();
            if (scoopPaging != null)
            {
                _currentComponent = Component.Paggin;
                Evaluate(scoopPaging);
            }


            return assembleTranslations();
        }
        void Evaluate(Expression node, object entity = null, IDataTablesRequest DataTablesRequest = null)
        {
            switch (_currentComponent)
            {
                case Component.Where:
                    prepareWhere(node);
                    break;
                case Component.CreatedSet:

                case Component.OrderBy:
                    prepareOrderBy(node);
                    break;
                case Component.OrderByDesc:
                    prepareOrderByDesc(node);
                    break;
                case Component.Filtre:
                    prepareFiltre(entity);
                    break;
                case Component.DataTablesRequest:
                    if (DataTablesRequest == null)
                        break;
                    prepareDataTablesRequest(DataTablesRequest);
                    break;
                case Component.Count:
                    break;
            }
        }
        void Evaluate(ScoopPagging pagePagging)
        {
            switch (_currentComponent)
            {
                case Component.Paggin:
                    preparePaggin(pagePagging);
                    break;
            }
        }
        void preparePaggin(ScoopPagging node)
        {

            new Translation(getComponentValue(Component.Paggin), node.Page.ToString())
                .Finally(_translations.Add);

            new Translation(getComponentValue(Component.Paggin), node.PageCount.ToString())
                .Finally(_translations.Add);

        }
        void prepareDataTablesRequest(IDataTablesRequest node)
        {
            if (node == null)
                return;


            if (!string.IsNullOrEmpty(node.Search.Value))
            {
                string str = "(";
                foreach (string str2 in (from x in node.Columns select x.Data).ToArray<string>())
                {
                    if (!string.IsNullOrEmpty(str2))
                    {
                        str = str + string.Format("{0} Like('%{1}%') or ", str2, node.Search.Value);
                    }
                }
                str = str.Substring(0, str.Length - 3) + ")";
                new Translation(getComponentValue(Component.Where), str).Finally(_translations.Add);
            }
            if (_components.Count(x => (x.ComponentType == Component.Count)) <= 0)
            {
                var column = (from x in node.Columns
                              where x.IsOrdered
                              select x).FirstOrDefault();
                if (column != null)
                {
                    if (column.SortDirection == Column.OrderDirection.Ascendant)
                    {
                        this.prepareOrderBy(null, false, column.Data);
                    }
                    else
                    {
                        this.prepareOrderByDesc(null, false, column.Data);
                    }
                }
                var pagging = new ScoopPagging { Page = node.Start, PageCount = node.Length };
                preparePaggin(pagging);
                getComponentValue(Component.DataTablesRequest);
            }

        }
        void prepareFiltre(object node)
        {
            var val = node.GetFiltreMySqlWhere();
            _parameter = val._MysqlParameters;
            new Translation(getComponentValue(Component.Filtre), val.sb.ToString()).Finally(_translations.Add);
        }
        void prepareOrderBy(Expression node, bool orderS = true, string key = "")
        {
            var order = _components.Any(x => x.DataTablesRequest != null && x.DataTablesRequest.Columns.Count(c => c.IsOrdered) > 0);
            if (order && orderS)
            {
                return;
            }


            var val = node == null && !string.IsNullOrEmpty(key)
                ? key
                : (node == null
                    ? _components.Where(x => x.ComponentType == Component.OrderBy)
                        .Select(x => x.Creator)
                        .FirstOrDefault()
                    : node).GetValue<T>();


            var currentOrderBy = _translations.FirstOrDefault(x => x.Type == _ORDERBY);

            if (currentOrderBy != null)
                currentOrderBy.Value = val.ToString();
            else
                new Translation(getComponentValue(Component.OrderBy), val.ToString())
                    .Finally(_translations.Add);

        }
        void prepareOrderByDesc(Expression node, bool orderS = true, string key = "")
        {
            var order = _components.Any(x => x.DataTablesRequest != null && x.DataTablesRequest.Columns.Count(c => c.IsOrdered) > 0);
            if (order && orderS)
            {
                return;
            }

            var val = node == null && !string.IsNullOrEmpty(key)
                ? key
                : (node == null
                    ? _components.Where(x => x.ComponentType == Component.OrderByDesc)
                        .Select(x => x.Creator)
                        .FirstOrDefault()
                    : node).GetValue<T>();

            var currentOrderBy = _translations.FirstOrDefault(x => x.Type == _ORDERBYDESC);

            if (currentOrderBy != null)
                currentOrderBy.Value = val.ToString();
            else
                new Translation(getComponentValue(Component.OrderByDesc), val.ToString())
                    .Finally(_translations.Add);
        }
        void prepareWhere(Expression node)
        {
            var obj = node.GetValueMySqlWhere<T>(_parameter);
            setTranslation(obj);
        }
        byte getComponentValue(Component component)
        {
            switch (component)
            {
                case Component.From:
                    return _FROM;

                case Component.Where:
                    return _WHERE;

                case Component.OrderBy:
                    return _ORDERBY;

                case Component.LikeLeft:
                    return _LikeLeft;

                case Component.LikeIn:
                    return _LikeIn;

                case Component.LikeRight:
                    return _LikeRight;

                case Component.Between:
                    return _Between;

                case Component.Paggin:
                    return _Paggin;

                case Component.OrderByDesc:
                    return _ORDERBYDESC;
                case Component.Filtre:
                    return _Filtre;
                case Component.DataTablesRequest:
                    return _DataTablesRequest;
                case Component.Count:
                    return _Count;
                default:
                    throw new PercolatorException(
                        $"The component type '{component}' durring the PAS tanslation is not valid");
            }
        }
        string assembleTranslations()
        {
            //var sb = new StringBuilder(Comment.PAS_HEADER).AppendLine();
            var sb = new StringBuilder();
            sb.AppendLine();
           
            sb.AppendLine("SELECT");

            var countSuccess = _components.Count(x => x.ComponentType == Component.Count) > 0;
            if (!countSuccess)
            {
                sb.AppendLine(" * ");
            }
            if (countSuccess)
            {
                sb.AppendLine(" COUNT(*) ");
            }

            sb.AppendLine(_translations.First(x => x.Type == _FROM).Value);

            var slicers = _translations.Where(x => x.Type == _WHERE);
            var slicerCount = slicers.Count();
            if (slicerCount > 0)
            {
                //sb.AppendLine(Comment.FOR_SLICER_REGION);
                sb.AppendLine("WHERE\r\n");
                slicers
               .Select(x => x.Value)
               .Aggregate((a, b) => $"\t{a} and \r\n\t{b}")
               .To(sb.AppendLine);
            }


            var filtre = _translations.Where(x => x.Type == _Filtre);
            var filtreCount = filtre.Count();
            if (filtreCount > 0)
            {
                //sb.AppendLine(Comment.FOR_SLICER_REGION);
                sb.AppendLine("WHERE\r\n");
                filtre
               .Select(x => x.Value)
               .Aggregate((a, b) => $"\t{a},\r\n\t{b}")
               .To(sb.AppendLine);
            }


            var orderBy = _translations.Where(x => x.Type == _ORDERBY);
            if (orderBy.Count() > 0)
            {
                sb.AppendLine(" ORDER BY ");
                orderBy
               .Select(x => x.Value)
               .Aggregate((a, b) => $"\t{a}")
               .To(sb.AppendLine);
                sb.AppendLine(" ASC ");
            }


            var orderByDesc = _translations.Where(x => x.Type == _ORDERBYDESC);
            if (orderByDesc.Count() > 0)
            {
                sb.AppendLine(" ORDER BY ");
                orderByDesc
               .Select(x => x.Value)
               .Aggregate((a, b) => $"\t{a}")
               .To(sb.AppendLine);
                sb.AppendLine(" DESC ");
            }

            /*SELECT column FROM table LIMIT {someLimit} OFFSET {someOffset};*/
            var paggin = _translations.Where(x => x.Type == _Paggin);
            if (paggin.Count() > 0)
            {
                paggin
               .Select(x => x.Value)
               .Aggregate((a, b) => $"\t LIMIT {a},{b} ")
               .To(sb.AppendLine);
            }


            return sb.ToString();
        }
        void setTranslation(object obj)
        {
            if (obj != null)
            {
                if (obj is IEnumerable<object>)
                    foreach (var o in (IEnumerable<object>)obj)
                        _translations.Add(new Translation(_currentAxis, o.ToString()));
                else
                    _translations.Add(new Translation(_currentAxis, obj.ToString()));
            }
        }
    }
}
