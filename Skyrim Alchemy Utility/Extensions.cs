using System;
using System.Collections.Generic;
using System.Linq;

namespace Skyrim_Alchemy_Utility
{
    static class Extensions
    {
        public static IEnumerable<TElement> DistinctBy<TElement,TKey>(this IEnumerable<TElement> collection,Func<TElement,TKey> f)
        {
            return collection.GroupBy(f).Select(g => g.First());
        }
    }
}
