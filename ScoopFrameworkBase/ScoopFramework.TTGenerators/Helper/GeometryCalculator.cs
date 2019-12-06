using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.SqlServer.Types;

namespace ScoopFramework.TTGenerators.Helper
{
    public class GeometryCalculator
    {

        #region Private Methods

        private int R = 6371000;

        private class CalculatedPointList
        {
            public int pointIndex { get; set; }
            public System.Windows.Point point { get; set; }
            public double distance { get; set; }
        }

        private double ToDegrees(double x)
        {
            return x * 180 / Math.PI;
        }

        private double ToRadians(double x)
        {
            return x * Math.PI / 180;
        }

        private System.Windows.Point GetPointFromGeographyPoint(SqlGeography item)
        {
            return new System.Windows.Point { X = item.Lat.Value, Y = item.Long.Value };
        }

        //  LINESTRING      POLYGON     POINT
        private string GetCheckedGeographyString(string item)
        {

            if (item.IndexOf("POLYGON") == 0)   //  MULTIPOLYGON    farkı için
            {

                var itemStr = item.Replace("POLYGON ((", "").Replace("POLYGON((", "").Replace("))", "").Trim();
                var itemArr = itemStr.Split(',').ToList();

                if (itemArr.FirstOrDefault().Trim() != itemArr.LastOrDefault().Trim())
                {
                    return "POLYGON ((" + itemStr + ", " + itemArr.FirstOrDefault().Trim() + "))";
                }
                else
                {
                    return "POLYGON ((" + itemStr + "))";
                }

            }
            else if (item.IndexOf("LINESTRING (") > -1)
            {

                var itemStr = item.Replace("LINESTRING (", "").Replace("LINESTRING(", "").Replace(")", "").Trim();
                return "LINESTRING (" + itemStr + ")";

            }
            else if (item.IndexOf("POINT") > -1)
            {
                var itemStr = item.Replace("POINT(", "").Replace("POINT (", "").Replace(")", "").Trim();
                return "POINT (" + itemStr + ")";
            }

            return null;

        }

        private List<CalculatedPointList> GetCalculatedPointsFromPolygon(SqlGeography item)
        {

            var res = new List<CalculatedPointList>();

            for (int i = 1; i < item.STNumPoints(); i++)
            {
                var tp1 = item.STPointN(i);
                var tp2 = item.STPointN(i + 1);

                res.Add(new CalculatedPointList
                {
                    pointIndex = i - 1,
                    point = GetPointFromGeographyPoint(tp1),
                    distance = ComputeDistance(GetPointFromGeographyPoint(tp1), GetPointFromGeographyPoint(tp2))
                });

            }

            return res;

        }

        private SqlGeography GetPolygonFromCalculatedList(List<CalculatedPointList> items)
        {
            var sql = new SqlGeography();

            var polStr = "";

            for (int i = 0; i < items.Count(); i++)
            {
                polStr += ", " + items[i].point.X.ToString().Replace(",", ".") + " " + items[i].point.Y.ToString().Replace(",", ".");
            }

            polStr = "POLYGON ((" + polStr.Substring(2) + "))";

            return GetGeographyFromStr(polStr);

        }

        #endregion

        /// <summary>
        /// 2 Nokta ( Point ) Arasındaki Açıyı verir
        /// </summary>
        /// <param name="p1">1. Nokta</param>
        /// <param name="p2">2. Nokta</param>
        /// <returns>Derece ( Double )</returns>
        public double ComputeHeading(Point p1, Point p2)
        {

            var thisx = ToRadians(p1.X);
            var thisy = ToRadians(p1.Y);
            var pointx = ToRadians(p2.X);
            var pointy = ToRadians(p2.Y);

            var y = Math.Sin(pointy - thisy) * Math.Cos(pointx);
            var x = Math.Cos(thisx) * Math.Sin(pointx) -
                Math.Sin(thisx) * Math.Cos(pointx) * Math.Cos(pointy - thisy);
            var alpha = Math.Atan2(y, x);

            return (ToDegrees(alpha) + 360) % 360;

        }

        /// <summary>
        /// 2 Nokta ( Point ) Arasındaki Mesafeyi Verir
        /// </summary>
        /// <param name="p1">1. Nokta</param>
        /// <param name="p2">2. Nokta</param>
        /// <returns>Double ( Metre )</returns>
        public double ComputeDistance(System.Windows.Point p1, System.Windows.Point p2)
        {

            var dLat = ToRadians(p2.X - p1.X);
            var dLong = ToRadians(p2.Y - p1.Y);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(p1.X)) * Math.Cos(ToRadians(p2.X)) *
                    Math.Sin(dLong / 2) * Math.Sin(dLong / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;

        }

        /// <summary>
        /// 1 Noktadan x Mesafe Uzaklığa y Derece Açıyla olan Yeni Noktayı verir
        /// </summary>
        /// <param name="p">Nokta</param>
        /// <param name="distance">Mesafe ( x Metre )</param>
        /// <param name="angle">Açı ( y Derece )</param>
        /// <param name="radius">Açı için Angle değeri ( Boş Bırakılabilir )</param>
        /// <returns>Yeni Nokta ( Point )</returns>
        public System.Windows.Point ComputeOffset(Point p, double distance, double angle, double radius = 6371000)
        {

            var dist = distance / radius;
            var ang = ToRadians(angle);

            var px = ToRadians(p.X);
            var py = ToRadians(p.Y);

            var npx = Math.Asin(Math.Sin(px) * Math.Cos(dist) + Math.Cos(px) * Math.Sin(dist) * Math.Cos(ang));
            var npy = py + Math.Atan2(Math.Sin(ang) * Math.Sin(dist) * Math.Cos(px),
                    Math.Cos(dist) - Math.Sin(px) * Math.Sin(npx));

            npy = (npy + 3 * Math.PI) % (2 * Math.PI) - Math.PI;


            return new System.Windows.Point { X = ToDegrees(npx), Y = ToDegrees(npy) };
        }

        /// <summary>
        /// 2 Nokta ve Dakika verilerek ( KM/H ) hesaplaması
        /// </summary>
        /// <param name="p1">1. Nokta</param>
        /// <param name="p2">2. Nokta</param>
        /// <param name="Minute">Gidiş Süresi Dakika cinsinden</param>
        /// <returns>KMH ( double )</returns>
        public double ComputeKMH(System.Windows.Point p1, System.Windows.Point p2, double Minute)
        {

            var mesafe = ComputeDistance(p1, p2) * 0.001;       //      km   =   0.001

            var saatteKM = 60 * mesafe / Minute;

            return saatteKM;

        }

        /// <summary>
        /// 2 Nokta ( Point ) Arasında kalan noktayı verir.
        /// </summary>
        /// <param name="p1">1. Nokta</param>
        /// <param name="p2">2. Nokta</param>
        /// <returns>Nokta ( Point )</returns>
        public System.Windows.Point ComputeMiddlePoint(Point p1, Point p2)
        {

            var p1x = ToRadians(p1.X);
            var p1y = ToRadians(p1.Y);
            var p2x = ToRadians(p2.X);
            var p2y = ToRadians(p2.Y);
            var p2p1Y = ToRadians(p2.Y - p1.Y);

            var bx = Math.Cos(p2x) * Math.Cos(p2p1Y);
            var by = Math.Cos(p2x) * Math.Sin(p2p1Y);

            double a3 = Math.Atan2(Math.Sin(p1x) + Math.Sin(p2x),
                    Math.Sqrt((Math.Cos(p1x) + bx) * (Math.Cos(p1x) + bx) + Math.Pow(by, 2)));

            double t3 = p1y + Math.Atan2(by, Math.Cos(p1x) + bx);
            t3 = (t3 + 3 * Math.PI) % (2 * Math.PI) - Math.PI;      //  Normalize to -180 and +180

            return new System.Windows.Point { X = ToDegrees(a3), Y = ToDegrees(t3) };

        }

        /// <summary>
        /// Pointlerin bulunduğu bir listeyi SqlGeography tipine çevirir... POLYGON veya LINESTRING
        /// </summary>
        /// <param name="item">List şeklinde POINT</param>
        /// <param name="type">POLYGON    -   LINESTRING</param>
        /// <returns>SqlGeography</returns>
        public SqlGeography GetGeographyFromPoints(List<Point> item, string type)
        {
            var sqlStr = "";
            var sqlPol = new SqlGeography();
            var startEndString = new List<string>();

            if ((type != "POLYGON" && type != "LINESTRING") ||
                (type == "POLYGON" && item.Count() < 3) ||
                (type == "LINESTRING" && item.Count() < 2))
            {
                return null;
            }

            if (type == "POLYGON") { startEndString = new List<string> { "POLYGON ((", "))" }; }
            if (type == "LINESTRING") { startEndString = new List<string> { "LINESTRING (", ")" }; }

            foreach (var coord in item)
            {
                sqlStr += ", " + coord.X.ToString().Replace(",", ".") + " " + coord.Y.ToString().Replace(",", ".");
            }

            sqlStr = sqlStr.Substring(2);

            return GetGeographyFromStr(startEndString[0] + sqlStr + startEndString[1]);
        }

        public SqlGeography GetCheckedGeography(string item)
        {

            SqlGeography sqlGeog = new SqlGeography();

            try
            {
                sqlGeog = SqlGeography.STGeomFromText(new System.Data.SqlTypes.SqlChars(item), 4326);
            }
            catch (Exception)
            {
                return null;
            }

            if (!sqlGeog.STIsValid().IsTrue)
            {
                return null;
            }

            if (sqlGeog.EnvelopeAngle() >= 90)
            {
                sqlGeog = sqlGeog.ReorientObject();
            }

            return sqlGeog;

        }

        /// <summary>
        /// POLYGON verisini en uzun kenar birinci sıraya denk gelecek şekilde sıralar.
        /// </summary>
        /// <param name="item">SqlGeography</param>
        /// <returns>SqlGeography</returns>
        public SqlGeography PolygonSortByLongLine(SqlGeography item)
        {

            if (item.STGeometryType() != "Polygon") { return null; }

            var newList = new List<CalculatedPointList>();
            var pointList = GetCalculatedPointsFromPolygon(item).ToList();
            var firstPoint = Convert.ToInt32(pointList.OrderByDescending(a => a.distance).FirstOrDefault().pointIndex);

            var list1 = pointList.Skip(firstPoint).Take(pointList.Count() - firstPoint).ToList();
            var list2 = pointList.Take(firstPoint);

            foreach (var lx in list1)
            {
                newList.Add(lx);
            }
            foreach (var lx in list2)
            {
                newList.Add(lx);
            }

            return GetPolygonFromCalculatedList(newList);

        }

        /// <summary>
        /// String türündeki veriyi düzenleyip dönüş açısı, valid vs formüllere tabi tutup geometry gönderir.
        /// </summary>
        /// <param name="item">"POLYGON (( xy, xy ))"   -   "LINESTRING ( xy, xy )"  -   "POINT ( xy )"</param>
        /// <returns>SqlGeography .IsNull kontrolü yapılmalı.</returns>
        public SqlGeography GetGeographyFromStr(string item)
        {

            if (String.IsNullOrEmpty(item)) { return null; }

            item = GetCheckedGeographyString(item);

            return GetCheckedGeography(item);

        }

        /// <summary>
        /// Polygon tipindeki verinin Boundingboxlarını verir
        /// </summary>
        /// <param name="item">Polygon ( SqlGeography )</param>
        /// <returns>4 Point ( sol üst , sağ üst , sağ alt , sol alt )</returns>
        public List<System.Windows.Point> GetBoundingBox(SqlGeography item)
        {

            if (item.IsNull || item.STIsValid().IsFalse || item.STGeometryType() != "Polygon")
            {
                return null;
            }

            if (item.EnvelopeAngle() >= 90) { item = item.ReorientObject(); }

            var list = new List<CalculatedPointList>();

            for (int i = 1; i < item.STNumPoints(); i++)
            {
                list.Add(new CalculatedPointList
                {
                    pointIndex = i - 1,
                    point = GetPointFromGeographyPoint(item.STPointN(i))
                });

            }

            var res = new List<System.Windows.Point>();

            res.Add(new System.Windows.Point { X = list.Min(a => a.point.X), Y = list.Max(a => a.point.Y) });
            res.Add(new System.Windows.Point { X = list.Max(a => a.point.X), Y = list.Max(a => a.point.Y) });
            res.Add(new System.Windows.Point { X = list.Max(a => a.point.X), Y = list.Min(a => a.point.Y) });
            res.Add(new System.Windows.Point { X = list.Min(a => a.point.X), Y = list.Min(a => a.point.Y) });

            return res;

        }
    }
}
