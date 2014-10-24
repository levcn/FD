using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SLControls.Extends
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, IEnumerable<T> second)
        {
            foreach (T obj in source) yield return obj;
            foreach (T obj in second) yield return obj;
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) return;
            foreach (T obj in source) action(obj);
        }

        public static bool TrueForEach<T>(this IEnumerable<T> source, Func<T, bool> action)
        {
            return source.All(action);
        }

        public static int FindIndex<T>(this IList<T> source, Predicate<T> match)
        {
            int num = -1;
            foreach (T obj in source)
            {
                ++num;
                if (match(obj)) return num;
            }
            return -1;
        }
    }
}
