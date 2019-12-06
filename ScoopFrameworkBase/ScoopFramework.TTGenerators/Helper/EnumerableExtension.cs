using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoopFramework.TTGenerators.Helper
{
    public static class EnumerableExtension
    {


        public static IEnumerable<TSource> Difference<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> other)
        {
            foreach (var item in source)
            {
                if (!other.Contains(item))
                    yield return item;
            }
        }
        public static int IndexOf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> res)
        {

            int i = 0;
            if (res != null)
                foreach (var item in source)
                {
                    if (res(item)) return i;
                    i++;
                }
            return -1;
        }


        public static int IndexOf<T>(this IEnumerable<T> source, T value)
        {
            return source.IndexOf(value, EqualityComparer<T>.Default);
        }

        public static int IndexOf<T>(this IEnumerable<T> source, T value, EqualityComparer<T> comparer)
        {
            var index = 0;
            foreach (var item in source)
            {
                if (comparer.Equals(value, item)) return index;
                index++;
            }
            return -1;
        }
        public static TSource ItemOrDefault<TSource>(this IEnumerable<TSource> source, int index)
        {

            int i = 0;
            foreach (var item in source)
            {
                if (i == index) return item;
                i++;
            }
            return default(TSource);
        }
        public static string FormatJoin<TSource>(this IEnumerable<TSource> source, string format)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in source)
                sb.AppendFormat(format, item);
            return sb.ToString();
        }
        public static string FormatJoin<TSource>(this IEnumerable<TSource> source, string format, string seperator)
        {
            return FormatJoin(source, format, seperator, "{0}");

        }
        public static string FormatJoin<TSource>(this IEnumerable<TSource> source, string format, string seperator, string outformat)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in source)
            {
                if (sb.Length > 0)
                    sb.Append(seperator);
                sb.AppendFormat(format, item);

            }
            return sb.Length > 0 ? string.Format(outformat, sb.ToString()) : "";
        }
        public static string FormatJoin<TSource>(this IEnumerable<TSource> source, Func<TSource, string> format, string seperator, string outformat)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in source)
            {
                if (sb.Length > 0)
                    sb.Append(seperator);
                sb.Append(format(item));
            }

            return string.IsNullOrEmpty(outformat) ? sb.ToString() : string.Format(outformat, sb.ToString());
        }

        public static void Do<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (action != null)
                foreach (var item in source)
                {
                    action(item);
                }
        }

    }
}
