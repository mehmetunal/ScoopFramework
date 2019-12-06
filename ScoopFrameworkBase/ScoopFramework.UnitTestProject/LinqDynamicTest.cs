using System.Collections.Generic;
using System.Linq;
using ScoopFramework.LinqDynamic;

namespace ScoopFramework.UnitTestProject
{
    public class LinqDynamicTest
    {
        /*LinqDynamic Kullanımı*/
        public string LinqDynamicWhere()
        {
            var model = new List<MyClass>();

            model.AsQueryable().Where("asdasdasd");

            return "";
        }
    }

    class MyClass
    {
        public string Name { get; set; }
    }
}
