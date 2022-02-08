namespace Data.Extensions
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
