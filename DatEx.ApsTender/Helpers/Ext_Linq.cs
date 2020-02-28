using System;
using System.Collections.Generic;
using System.Text;

namespace DatEx.ApsTender.Helpers
{
    public static class Ext_Linq
    {
        public static IEnumerable<IEnumerable<T>> Paginate<T>(this IEnumerable<T> source, Int32 pageSize)
        {
            IEnumerator<T> enumerator = source.GetEnumerator();
            while(enumerator.MoveNext()) yield return NextPartition(enumerator, pageSize);
        }

        private static IEnumerable<T> NextPartition<T>(IEnumerator<T> enumerator, Int32 blockSize)
        {
            do
            {
                yield return enumerator.Current;
            } while(--blockSize > 0 && enumerator.MoveNext());
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach(TSource element in source)
                if(element != null && seenKeys.Add(keySelector(element)))
                    yield return element;
        }

        public static IEnumerable<TKey> DistinctValues<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> values = new HashSet<TKey>();
            foreach(TSource element in source)
                if(element != null && values.Add(keySelector(element)))
                    yield return keySelector(element);
        }
    }
}
