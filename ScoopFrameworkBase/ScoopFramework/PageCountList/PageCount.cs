using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoopFramework.PageCountList
{
   public static class PageCount
    {
       public static List<int> PageLinkCount(int prm)
       {
           var dParam = new List<int>();
           for (int i = 1; i <= prm; i++)
           {
               dParam.Add(i);
           }

           return dParam;
       }
    }
}
