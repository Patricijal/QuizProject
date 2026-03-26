using System;
using System.Collections.Generic;

public static class EnumerableExtensions
{
    // You created your own generic extension method (1 point)
    public static IEnumerable<T> TakeSafe<T>(this IEnumerable<T> source, int count)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (count <= 0)
        {
            yield break;
        }

        int taken = 0;
        foreach (var item in source)
        {
            if (taken++ >= count)
            {
                yield break;
            }

            yield return item;
        }
    }
}
