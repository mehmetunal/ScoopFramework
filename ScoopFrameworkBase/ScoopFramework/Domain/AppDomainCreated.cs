using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace ScoopFramework.Domain
{
    public static class AppDomainCreated
    {
        public static void PreLoadDeployedAssemblies()
        {
            foreach (var path in GetBinFolders())
            {
                PreLoadAssembliesFromPath(path);
            }
        }
        private static IEnumerable<string> GetBinFolders()
        {
            List<string> toReturn = new List<string>();
            if (HttpContext.Current != null)
            {
                toReturn.Add(HttpRuntime.BinDirectory);
            }
            else
            {
                toReturn.Add(AppDomain.CurrentDomain.BaseDirectory);
            }

            return toReturn;
        }

        private static void PreLoadAssembliesFromPath(string p)
        {
            FileInfo[] files = null;
            files = new DirectoryInfo(p).GetFiles("*.dll", SearchOption.AllDirectories);

            AssemblyName a = null;
            foreach (var fi in files)
            {
                var s = fi.FullName;
                a = AssemblyName.GetAssemblyName(s);
                if (!AppDomain.CurrentDomain.GetAssemblies().Any(assembly =>
                  AssemblyName.ReferenceMatchesDefinition(a, assembly.GetName()))) { Assembly.Load(a); }
            }
        }

    }
}
