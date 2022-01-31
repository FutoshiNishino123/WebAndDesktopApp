using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class LinqExtensions
    {
        public static T ElementAtRandom<T>(this IEnumerable<T> collection)
        {
            var index = Random.Shared.Next(collection.Count());
            return collection.ElementAt(index);
        }
    }
}
