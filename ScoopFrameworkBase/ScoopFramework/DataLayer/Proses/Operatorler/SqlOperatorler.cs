using System.Collections.Generic;
using System.Linq;
using ScoopFramework.Enum;
using ScoopFramework.Model;

namespace ScoopFramework.DataLayer.Proses.Operatorler
{
    public struct SqlOperatorler
    {
        public static string SqlWhere(List<ParamValue> paramValues, string table, ParamValue whereSucces)
        {
            var sql = string.Format("{0} * {1} {2} {3} {4} ", SqlQueryEnum.SELECT, SqlQueryEnum.FROM, table,
                whereSucces != null ? SqlQueryEnum.WHERE.ToString() : string.Empty,
                whereSucces != null
                    ? string.Join(SqlQueryEnum.AND.ToString(),
                        paramValues.Select(p => p.Where)
                            .SelectMany(whr => whr)
                            .Select(p => string.Format(" {0}=@{1} ", p.Key, p.Key.Replace("<", " ").Replace(">", " "))))
                    : "");
            return sql;
        }
        public static string SqlPagging(List<ParamValue> paramValues, string sql)
        {
            var pageing = paramValues.Select(value => value.page).FirstOrDefault();
            if (pageing != null)
            {
                sql += string.Format("  OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", pageing.start, pageing.end);
            }
            return sql;
        }
        public static string SqlOrderByDesc(List<ParamValue> paramValues, string sql)
        {
            var orderByDesc = paramValues.Select(value => value.OrderByDescending).ToList();
            if (orderByDesc.FirstOrDefault() != null && orderByDesc.Any())
            {
                var desc = string.Empty;
                var orDefault = orderByDesc.FirstOrDefault();
                if (orDefault != null)
                    desc += orDefault + " DESC,";
                sql += string.Format("ORDER BY {0} ", desc.TrimEnd(','));
            }
            return sql;
        }
        public static string SqlLike(List<ParamValue> paramValues, ParamValue likeSucces, ParamValue whereSucces, string sql)
        {
            if (likeSucces != null)
            {
                if (whereSucces == null)
                {
                    sql += " Where ";
                }

                sql += " " + string.Join(SqlQueryEnum.AND.ToString(),
                    paramValues.Select(p => p.Like)
                        .SelectMany(lk => lk)
                        .Select(p => string.Format(" {0} LIKE '{1}%' ", p.Key, p.Value)));
            }
            return sql;
        }
        public static string SqlOrderByAsc(List<ParamValue> paramValues, string sql)
        {
            var orderByAsc = paramValues.Select(value => value.OrderByAsc).FirstOrDefault();
            if (orderByAsc != null && orderByAsc.Any())
            {
                sql += " ORDER BY " + orderByAsc + " ASC";
            }
            return sql;
        }
        public static string SqlBetween(List<ParamValue> paramValues, string sql)
        {
            var betWeenList =
                paramValues.FirstOrDefault(p => !string.IsNullOrEmpty(p.Between) && p.Where == null);
            if (betWeenList != null && betWeenList.Between.Any())
            {
                var between = betWeenList.Between.EndsWith("AND")
                    ? betWeenList.Between.Substring(0, betWeenList.Between.Length - 3)
                    : betWeenList.Between;
                sql += " Where " + between;
            }
            return sql;
        }
        public static string SqlAnd(ParamValue firstOrDefault, string sql)
        {
            if (firstOrDefault != null && firstOrDefault.Between.Any())
            {
                var between = firstOrDefault.Between.EndsWith("AND")
                    ? firstOrDefault.Between.Substring(0, firstOrDefault.Between.Length - 3)
                    : firstOrDefault.Between;
                sql += " AND " + between;
            }
            return sql;
        }
        public static string SqlWhereCount<T>(List<ParamValue> paramValues, string table, ParamValue whereSucces) where T : new()
        {
            var sql = string.Format("{0} Count(*) as Deger {1} {2} {3} {4} ", SqlQueryEnum.SELECT, SqlQueryEnum.FROM, table,
                whereSucces != null ? SqlQueryEnum.WHERE.ToString() : string.Empty,
                whereSucces != null
                    ? string.Join(SqlQueryEnum.AND.ToString(),
                        paramValues.Select(p => p.Where)
                            .SelectMany(whr => whr)
                            .Select(p => string.Format(" {0}=@{1} ", p.Key, p.Key.Replace("<", " ").Replace(">", " "))))
                    : "");
            return sql;
        }
    }
}
