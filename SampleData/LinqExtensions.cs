using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class LinqExtensions
{
    public static T ElementAtRandom<T>(this IEnumerable<T> collection)
    {
        return collection.ElementAt(Random.Shared.Next(collection.Count()));
    }
}
