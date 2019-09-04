using System;
using System.Collections.Generic;

namespace Comet.Internal
{
    public static class ListExtensions
    {
       public static T SafeGetAtIndex<T>(this IReadOnlyList<T> list, int index, Func<int,T> getItem = null)
        {
            if(index >= 0 &&  index < (list?.Count ?? 0))
                return list[index];
            if (getItem != null)
                return getItem(index);
            return default;

        }
    }
}
