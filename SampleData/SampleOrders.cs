using Microsoft.EntityFrameworkCore;
using Data;
using Data.Models;
using Common.Extensions;
using Common.Utils;

namespace SampleData
{
    internal class SampleOrders
    {
        private readonly AppDbContext _db;

        public SampleOrders(AppDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Order> CreateData(int count)
        {
            var expiration = new RandomDate(DateTime.Today.AddDays(-30), DateTime.Today.AddDays(30));
            var people = _db.Users.ToArray();
            var statuses = _db.Statuses.ToArray();

            var orders = Enumerable.Range(0, count).Select(i =>
            {
                var order = new Order
                {
                    Expiration = i % 3 == 0 ? expiration.Next() : null,
                    Number = "SN" + i.ToString("D4"),
                    User = people?.ElementAtRandom(),
                    Status = statuses?.ElementAtRandom(),
                    Remarks = "テスト案件",
                    IsClosed = i % 5 == 0,
                };

                return order;
            });

            return orders;
        }

        public void AddData(int count)
        {
            var data = CreateData(count);
            _db.AddRange(data);
            _db.SaveChanges();
        }

        public void PrintData()
        {
            var orders = _db.Orders
                .Include(o => o.User)
                .Include(o => o.Status)
                .ToList();

            Console.WriteLine("--- Orders ---");
            Console.WriteLine("番号, 作成, 更新, 担当者, ステータス");
            foreach (var o in orders)
            {
                Console.WriteLine($"{o.Number}, {o.Created}, {o.Updated}, {o.User?.Name ?? "null"}, {o.Status?.Text ?? "null"}");
            }
            Console.WriteLine("--- /Orders ---");
        }
    }
}