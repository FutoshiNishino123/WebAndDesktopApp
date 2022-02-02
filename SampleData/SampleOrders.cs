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
                    Number = "SN" + i.ToString("D8"),
                    User = people?.ElementAtRandom(),
                    Status = statuses?.ElementAtRandom(),
                    Remarks = "テスト案件",
                    IsClosed = i % 5 == 0,
                };

                return order;
            });

            return orders;
        }

        public void AddData(int count = 100)
        {
            var data = CreateData(count);
            var hash = 1000;
            var done = 0;
            var i = 0;
            var start = DateTime.Now;
            while (true)
            {
                var index = hash * i++;
                var tmp = data.Skip(index).Take(hash);
                if (!tmp.Any())
                {
                    break;
                }

                _db.AddRange(tmp);
                _db.SaveChanges();

                done += tmp.Count();
                var remain = count - done;
                var ratio = (double)done / count * 100;
                var span = DateTime.Now - start;
                var estimate = span * remain / done;
                Console.Write($"{span.Minutes:#,0} 分 {span.Seconds:#,00} 秒 経過: " +
                              $"{done:#,0} / {count:#,0} 件完了 ({ratio:0.0} %), " +
                              $"あと {estimate.Minutes:#,0} 分 {estimate.Seconds:#,00} 秒");

                Console.SetCursorPosition(0, Console.CursorTop);
            }
            Console.WriteLine();
        }

        public void PrintData(int count = 10)
        {
            var orders = _db.Orders
                .Include(o => o.User)
                .Include(o => o.Status)
                .Take(count)
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