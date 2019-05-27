using System;
using System.Collections.Generic;

namespace TreeCollections
{
    internal static class CollectionExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> performAction)
        {
            foreach (var item in sequence)
            {
                performAction(item);
            }
        }
    }
}
