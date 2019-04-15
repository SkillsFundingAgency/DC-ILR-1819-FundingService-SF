using System;
using System.Collections.Generic;
using System.Linq;

namespace ESFA.DC.ILR.FundingService.Data.Extensions
{
    public static class EnumerableExtensions
    {
        public static HashSet<T> ToCaseInsensitiveHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source, StringComparer.OrdinalIgnoreCase as IEqualityComparer<T>);
        }

        public static Dictionary<string, TValue> ToCaseInsensitiveDictionary<TValue>(this IDictionary<string, TValue> dictionary)
        {
            return new Dictionary<string, TValue>(dictionary, StringComparer.OrdinalIgnoreCase);
        }

        public static Dictionary<string, TValue> ToCaseInsensitiveDictionary<TValue, TInput>(this IEnumerable<TInput> enumerable, Func<TInput, string> keySelector, Func<TInput, TValue> valueSelector)
        {
            return enumerable.ToDictionary(keySelector, valueSelector, StringComparer.OrdinalIgnoreCase);
        }

        public static IEnumerable<IEnumerable<T>> SplitList<T>(this IEnumerable<T> source, int nSize)
        {
            var l = source.ToList();

            for (var i = 0; i < l.Count; i += nSize)
            {
                yield return l.GetRange(i, Math.Min(nSize, l.Count - i));
            }
        }
    }
}