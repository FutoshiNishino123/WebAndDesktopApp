using Data;
using Data.Models;

namespace SampleData
{
    internal static class SampleStatuses
    {
        public static IEnumerable<Status> CreateData()
        {
            var obj = Data.RootobjectLoader.Load();

            var statuses = obj?.status.items.Select((s, i) => new Status
            {
                Id = i + 1,
                Text = s,
            });

            return statuses ?? Enumerable.Empty<Status>();
        }

        public static void AddData()
        {
            var statuses = CreateData();

            using var db = new AppDbContext();
            
            db.AddRange(statuses);
            
            db.SaveChanges();
        }

        public static void PrintData()
        {
            using var db = new AppDbContext();
            
            var statuses = db.Statuses.ToList();
            
            Console.WriteLine("--- Statuses ---");
            foreach (var s in statuses)
            {
                Console.WriteLine($"{s.Text}");
            }
            Console.WriteLine("--- /Statuses ---");
        }
    }
}