using System;

namespace ScoopFramework.TTGenerators.Helper
{
    public static class ObjectDetails
    {
        public static int? toInt32(this object nesne)
        {
            int? sonuc = null;
            try
            {
                sonuc = Convert.ToInt32(nesne);
            }
            catch (Exception)
            {
            }
            return sonuc;
        }
        public static double? toDouble(this object nesne)
        {
            double? sonuc = null;
            try
            {
                sonuc = Convert.ToDouble(nesne);
            }
            catch (Exception)
            {
            }
            return sonuc;
        }
        public static DateTime? toDate(this object nesne)
        {
            DateTime? sonuc = null;
            try
            {
                sonuc = Convert.ToDateTime(nesne);
            }
            catch (Exception)
            {
                return null;
            }
            return sonuc;
        }
    }
}
