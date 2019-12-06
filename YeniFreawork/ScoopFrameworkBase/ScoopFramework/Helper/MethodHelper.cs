using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ScoopFramework.Helper
{
    public class MethodHelper
    {
        public static IEnumerable<T> SetValuetoClass<T>(string classname)
        {

            List<T> res = new List<T>();
            //var ids = id.Split(',').ToList();
            var cls = GetDbClassFromString(classname);

            //foreach (var _id in ids)
            //{
            dynamic newClass = (dynamic)Activator.CreateInstance(cls);

            //foreach (var p in newClass.GetType().GetProperties())
            //{
            //    if (p.Name == "id" || p.Name == "Id")
            //    {
            //        p.SetValue(newClass, Guid.NewGuid());
            //    }
            //}

            res.Add(newClass);

            //}

            return res;

        }
        public static Type GetDbClassFromString(string className)
        {
            var paths = DLLHelper.DllPath.ToArray();
            var classlar = new List<Type>();
            foreach (var path in paths)
            {
                classlar = GetClassesInNamespace(path.Keys.ToString(), path.Keys.ToString());
                if (classlar.All(x => x.Name != className))
                {
                    classlar = GetClassesInNamespace(path.Keys.ToString(), path.Keys.ToString());
                }
            }
            return classlar.FirstOrDefault(a => a.Name.ToLower() == className.ToLower());
        }
        private static List<Type> GetClassesInNamespace(string dll, string nameSpace)
        {
            if (string.IsNullOrEmpty(nameSpace))
            {
                return new List<Type>();
            }
            try
            {
                var assemblys = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.ManifestModule.Name == String.Format("{0}.dll", dll)).ToList();
                foreach (var assembly in assemblys)
                {
                    var _type = assembly.GetTypes().Where(x => x.IsClass && x.Namespace == nameSpace).ToList();
                    if (_type.Count > 0)
                    {
                        return _type;
                    }
                }

            }
            catch (System.Exception ex)
            {
                if (ex is ReflectionTypeLoadException)
                {
                    var typeLoadException = ex as ReflectionTypeLoadException;
                    try
                    {
                        var assemblys = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.ManifestModule.Name == String.Format("{0}.dll", dll)).ToList();
                        foreach (var assembly in assemblys)
                        {
                            var _type = assembly.GetTypes().Where(x => x.IsClass && x.Namespace == nameSpace).ToList();
                            if (_type.Count > 0)
                            {
                                return _type;
                            }
                        }
                        var loaderExceptions = typeLoadException.LoaderExceptions;
                    }
                    catch { }
                }
            }
            return new List<Type>(); ;

        }
    }
}
