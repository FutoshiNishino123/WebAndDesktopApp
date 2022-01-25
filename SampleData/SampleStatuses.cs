using Data;
using Data.Models;

namespace SampleData
{
    internal class SampleStatuses
    {
        private readonly AppDbContext _db;

        public SampleStatuses(AppDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Status> CreateData()
        {
            var obj = Data.RootobjectLoader.Load();
            if (obj == null)
            {
                return Enumerable.Empty<Status>();
            }

            var statuses = obj.status.items.Select(s => new Status
            {
                Text = s,
            });

            return statuses;
        }

        public void AddData()
        {
            var statuses = CreateData();
            _db.AddRange(statuses);
            _db.SaveChanges();
        }

        public void PrintData()
        {
            var statuses = _db.Statuses.ToList();
            
            Console.WriteLine("--- Statuses ---");
            foreach (var s in statuses)
            {
                Console.WriteLine($"{s.Text}");
            }
            Console.WriteLine("--- /Statuses ---");
        }
    }
}