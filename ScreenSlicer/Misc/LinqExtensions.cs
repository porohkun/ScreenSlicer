using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var entry in source)
            {
                action?.Invoke(entry);
                yield return entry;
            }
        }

        public static IEnumerable<object> Where(this IEnumerable source, Func<object, bool> predicate)
        {
            foreach (var entry in source)
                if (predicate?.Invoke(entry) ?? false)
                    yield return entry;
        }
    }
}
