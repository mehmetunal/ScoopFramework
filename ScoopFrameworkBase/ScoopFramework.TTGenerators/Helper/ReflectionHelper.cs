using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ScoopFramework.TTGenerators.Helper
{
    public static class ReflectionHelper
    {
        static ReflectionHelper() { }

        public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class
        {
            List<T> objects = new List<T>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add((T)Activator.CreateInstance(type, constructorArgs));
            }
            return objects;
        }

        public static IEnumerable<Type> GetInheritedTypes<T>() where T : class
        {
            return Assembly.GetAssembly(typeof(T)).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)));
        }

        public static Type GetTypeByName(Type t, string name)
        {
            var result = Assembly.GetAssembly(t).GetTypes().Where(myType => myType.Name == name).FirstOrDefault();
            return result;
        }

        public static Type GetTypeByName<T>(string name)
        {
            var t = typeof(T);
            return GetTypeByName(t, name);
        }

        public static bool IsEnum(Type t)
        {
            if (t.IsEnum) return true;
            return t.IsGenericType &&
                   t.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                   t.GetGenericArguments()[0].IsEnum;


        }

    }
}
