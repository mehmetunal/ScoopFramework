using System;

namespace ScoopFramework.Applications
{
    public static class PagingApplication
    {
        public static double PagingCount(int modelCount, int pageCount)
        {
            var count = Convert.ToDouble(Math.Round((double)modelCount / pageCount, 1));
            if (count <= pageCount)
            {
                count = 1;
            }
            return count;
        }
    }
}
