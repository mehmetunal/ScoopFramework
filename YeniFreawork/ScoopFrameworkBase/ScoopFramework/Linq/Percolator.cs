using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ScoopFramework.DataTables;
using ScoopFramework.Exception;
using ScoopFramework.Helper;
using ScoopFramework.Mapping;

namespace ScoopFramework.Linq
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

    internal class Percolator<T>
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
        //T _cube;
        List<Axis<object>> _axis;
        Axis<object> _currentAxisObject;
        List<ScoopComponent> _components;
        List<Translation> _translations;
        public List<SqlParameter> _parameter;
        byte _currentAxis;
        Component? _currentComponent;

        public string ScoopCommand { get; private set; }
        internal Percolator(List<Axis<object>> axis, List<ScoopComponent> components)
        {
            // _cube = typeof(T).GetCubeInstance<T>();

            _setDepth = 0;
            _memberDepth = 0;
            _translations = new List<Translation>();
            _components = components;
            _axis = axis;
            _currentAxis = axis.Count == 0 ? (byte)0 : axis.Min(x => x.AxisNumber);
            _currentComponent = null;
            _parameter = new List<SqlParameter>();
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

                _translations.Add(new Translation(_FROM, string.Format("FROM [{0}]  WITH(NOLOCK) ", from)));
            }
            catch (NullReferenceException e)
            {
                throw new PercolatorException(string.Format("The cube type of '{0}' is not queryable", typeof(T).Name));
            }

            _axis.ForEach(axis =>
            {
                _currentAxis = axis.AxisNumber;
                _currentAxisObject = axis;
                Evaluate(axis.Creator);
            });
            _currentAxis = 190;
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

                case Component.CreatedMember:
                    prepareCalculatedMember(node);
                    break;

                case Component.CreatedSet:
                    prepareCalculatedSet(node);
                    break;

                case Component.SubCube:
                    prepareSubCube(node);
                    break;

                case Component.OrderBy:
                    prepareOrderBy(node);
                    break;
                case Component.OrderByDesc:
                    prepareOrderByDesc(node);
                    break;

                case Component.LikeLeft:
                    prepareLikeLeft(node);
                    break;
                case Component.LikeIn:
                    prepareLikeIn(node);
                    break;
                case Component.LikeRight:
                    prepareLikeRight(node);
                    break;
                case Component.Between:
                    prepareBetween(node);
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
                default:
                    prepareAxis(node);
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
        void prepareWhere(Expression node)
        {
            var obj = node.GetValueWhere<T>(_parameter);
            setTranslation(obj);
        }

        void prepareCalculatedMember(Expression node)
        {
            var memberExp = ((LambdaExpression)node).Body as MemberExpression;
            var obj = node.GetValue<T>();
            var comp = _components.First(x => x.Creator == node && x.ComponentType == _currentComponent);
            string name = string.Empty;
            if (!tryGetTagName(memberExp, out name))
            {
                if (string.IsNullOrEmpty(comp.Name))
                    name = $"_member{_memberDepth++}";
                else
                    name = comp.Name;
            }
            if (comp.Axis.HasValue)
            {
                var axis = _axis.FirstOrDefault(x => x.AxisNumber == comp.Axis);
                if (axis != null)
                {
                    if (!axis.WithMembers.Contains(name))
                        axis.WithMembers.Add(name);
                }
                else
                {
                    var a = new Axis<object>(comp.Axis.Value);
                    a.WithMembers.Add(name);
                    _axis.Add(a);
                }
            }
            _translations.Add(new Translation(_currentAxis, obj.ToString()) { Name = name });
        }
        void prepareCalculatedSet(Expression node)
        {
            var memberExp = ((LambdaExpression)node).Body as MemberExpression;
            var obj = node.GetValue<T>();
            var comp = _components.First(x => x.Creator == node && x.ComponentType == _currentComponent);
            string name = string.Empty;
            if (!tryGetTagName(memberExp, out name))
            {
                if (string.IsNullOrEmpty(comp.Name))
                    name = $"_set{_setDepth++}";
                else
                    name = comp.Name;
            }
            if (comp.Axis.HasValue)
            {
                var axis = _axis.FirstOrDefault(x => x.AxisNumber == comp.Axis);
                if (axis != null)
                {
                    if (!axis.WithSets.Contains(name))
                        axis.WithSets.Add(name);
                }
                else
                {
                    var a = new Axis<object>(comp.Axis.Value);
                    a.WithSets.Add(name);
                    _axis.Add(a);
                }
            }
            _translations.Add(new Translation(_currentAxis, obj.ToString()) { Name = name });
        }
        void prepareSubCube(Expression node)
        {
            var val = node.GetValue<T>();
            var currentSubcube = _translations.FirstOrDefault(x => x.Type == _SUBCUBE);

            if (val is IEnumerable<object>)
            {
                var concact = ((IEnumerable<object>)val)
                    .Select(x => x.ToString())
                    .Aggregate((a, b) => $"{a}, {b}");

                if (currentSubcube != null)
                    currentSubcube.Value = concact;
                else
                    new Translation(getComponentValue(Component.SubCube), concact)
                        .Finally(_translations.Add);
            }

            else
            {
                if (currentSubcube != null)
                    currentSubcube.Value = val.ToString();
                else
                    new Translation(getComponentValue(Component.SubCube), val.ToString())
                        .Finally(_translations.Add);
            }
        }
        void preparePaggin(ScoopPagging node)
        {

            new Translation(getComponentValue(Component.Paggin), node.Page.ToString())
                .Finally(_translations.Add);

            new Translation(getComponentValue(Component.Paggin), node.PageCount.ToString())
                .Finally(_translations.Add);

        }
        void prepareOrderBy(Expression node, bool orderS = true, string key = "")
        {
            var order = _components.Any(x => x.DataTablesRequest != null && x.DataTablesRequest.Columns.Count(c => c.IsOrdered) > 0);
            if (order && orderS)
            {
                return;
            }


            var val = node == null && !string.IsNullOrEmpty(key) ? key : (node == null ? _components.Where(x => x.ComponentType == Component.OrderBy).Select(x => x.Creator).FirstOrDefault() : node).GetValue<T>();


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

            var val = node == null && !string.IsNullOrEmpty(key) ? key : (node == null ? _components.Where(x => x.ComponentType == Component.OrderByDesc).Select(x => x.Creator).FirstOrDefault() : node).GetValue<T>();

            var currentOrderBy = _translations.FirstOrDefault(x => x.Type == _ORDERBYDESC);

            if (currentOrderBy != null)
                currentOrderBy.Value = val.ToString();
            else
                new Translation(getComponentValue(Component.OrderByDesc), val.ToString())
                    .Finally(_translations.Add);
        }
        void prepareLikeLeft(Expression node)
        {
            var val = node.GetValue<T>();
            var currentOrderBy = _translations.FirstOrDefault(x => x.Type == _LikeLeft);

            if (currentOrderBy != null)
                currentOrderBy.Value = val.ToString();
            else
                new Translation(getComponentValue(Component.LikeLeft), val.ToString())
                    .Finally(_translations.Add);
        }
        void prepareLikeIn(Expression node)
        {
            var val = node.GetValue<T>();
            var currentOrderBy = _translations.FirstOrDefault(x => x.Type == _LikeIn);

            if (currentOrderBy != null)
                currentOrderBy.Value = val.ToString();
            else
                new Translation(getComponentValue(Component.LikeIn), val.ToString())
                    .Finally(_translations.Add);
        }
        void prepareLikeRight(Expression node)
        {
            var val = node.GetValue<T>();
            var currentOrderBy = _translations.FirstOrDefault(x => x.Type == _LikeRight);

            if (currentOrderBy != null)
                currentOrderBy.Value = val.ToString();
            else
                new Translation(getComponentValue(Component.LikeRight), val.ToString())
                    .Finally(_translations.Add);
        }
        void prepareBetween(Expression node)
        {
            var val = node.GetValue<T>();
            var currentOrderBy = _translations.FirstOrDefault(x => x.Type == _Between);

            if (currentOrderBy != null)
                currentOrderBy.Value = val.ToString();
            else
                new Translation(getComponentValue(Component.Between), val.ToString())
                    .Finally(_translations.Add);
        }
        void prepareFiltre(object node)
        {
            var val = node.GetFiltreWhere();
            _parameter = val._parameters;
            new Translation(getComponentValue(Component.Filtre), val.sb.ToString()).Finally(_translations.Add);
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

        void prepareAxis(Expression node)
        {
            var obj = node.GetValue<T>();
            setTranslation(obj, _currentAxisObject.IsNonEmpty);
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

        void setTranslation(object obj, bool isNonEmpty)
        {
            if (obj != null)
            {
                if (obj is IEnumerable<object>)
                    foreach (var o in (IEnumerable<object>)obj)
                        _translations.Add(new Translation(_currentAxis, o.ToString(), isNonEmpty));
                else
                    _translations.Add(new Translation(_currentAxis, obj.ToString(), isNonEmpty));
            }
        }

        byte getComponentValue(Component component)
        {
            switch (component)
            {
                case Component.From:
                    return _FROM;

                case Component.Where:
                    return _WHERE;

                case Component.CreatedMember:
                    return _WMEMBER;

                case Component.CreatedSet:
                    return _WSET;

                case Component.SubCube:
                    return _SUBCUBE;

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

        bool tryGetTagName(MemberExpression member, out string name)
        {
            if (member == null)
            {
                name = null;
                return false;
            }
            //var tagAtt = member.Member.GetCustomAttribute<TagAttribute>();
            //if (tagAtt != null)
            //{
            //    name = tagAtt.Tag;
            //    return true;
            //}
            name = null;
            return false;
        }

        string assembleTranslations()
        {
            //var sb = new StringBuilder(Comment.PAS_HEADER).AppendLine();
            var sb = new StringBuilder();
            sb.AppendLine();
            var members = _translations.Where(x => x.Type == _WMEMBER);
            var sets = _translations.Where(x => x.Type == _WSET);
            var combined = members.Union(sets).OrderBy(x => x.DeclarationOrder);

            if (combined.Count() > 0)
            {
                //sb.AppendLine(Comment.FOR_CREATED_REGION);
                sb.AppendLine("WITH");
                foreach (var com in combined.OrderBy(x => x.DeclarationOrder))
                {
                    string type = com.Type == _WMEMBER ? "MEMBER" : "SET";
                    if (com.Name.Contains("_set"))
                        //sb.AppendLine(Comment.FOR_NO_SET_NAME);
                        if (com.Name.Contains("_member"))
                            //    sb.AppendLine(Comment.FOR_NO_MEMBER_NAME);
                            sb.AppendLine($"{type} {com.Name} AS");
                    sb.AppendLine(com.Value);
                    sb.AppendLine();
                }
            }

            //sb.AppendLine(Comment.FOR_SELECT_REGION);
            sb.AppendLine("SELECT");

            var countSuccess = _components.Count(x => x.ComponentType == Component.Count) > 0;
            if (_axis.Count() != 0 && !countSuccess)
            {
                _axis
                        .OrderBy(x => x.AxisNumber)
                        .Select(x => x.ToString())
                        .Aggregate((a, b) => $"{a},\r\n{b}")
                        .To(sb.AppendLine);
            }
            else if (!countSuccess)
            {
                sb.AppendLine(" * ");
            }
            if (countSuccess)
            {
                sb.AppendLine(" COUNT(*) ");
            }

            //sb.AppendLine(Comment.FOR_FROM_REGION);
            var subCube = _translations.FirstOrDefault(x => x.Type == _SUBCUBE);
            if (subCube != null)
            {
                sb.AppendLine("FROM")
                    .AppendLine("(")
                    .AppendLine("\tSELECT")
                    .AppendLine("\t{0}", subCube.Value)
                    .AppendLine($"\t {_translations.First(x => x.Type == _FROM).Value}")
                    .AppendLine(")");
            }
            else
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

            var paggin = _translations.Where(x => x.Type == _Paggin);
            if (paggin.Count() > 0)
            {
                sb.AppendLine(" OFFSET  ");
                paggin
               .Select(x => x.Value)
               .Aggregate((a, b) => $"\t{a} ROWS FETCH NEXT {b} ")
               .To(sb.AppendLine);
                sb.AppendLine("  ROWS ONLY ");
            }


            return sb.ToString();
        }


    }
}
