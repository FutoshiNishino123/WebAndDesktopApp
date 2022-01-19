using Microsoft.EntityFrameworkCore;
using Data;
using Data.Models;
using Common.Extensions;
using Common.Utils;

namespace SampleData
{
    internal static class SampleOrders
    {
        private static Person[]? People { get; set; }
        private static Status[]? Statuses { get; set; }

        public static IEnumerable<Order> CreateData(int count)
        {
            var rand = new RandomDateTime(DateTime.Now.AddDays(-30), DateTime.Now.AddDays(30));

            for (var i = 0; i < count; i++)
            {
                var date = rand.Next();

                var order = new Order
                {
                    Id = i + 1,
                    CreatedDate = date,
                    UpdatedDate = date,
                    ExpirationDate = i % 3 == 0 ? rand.Next() : null,
                    Number = "SM" + (i + 1).ToString("D4"),
                    Person = People?.ElementAtRandom(),
                    Status = Statuses?.ElementAtRandom(),
                    Remarks = "テスト案件",
                    IsClosed = i % 5 == 0,
                };
                
                yield return order;
            }
        }

        public static void AddData(int count)
        {
            using var db = new AppDbContext();

            People = db.People.ToArray();
            Statuses = db.Statuses.ToArray();

            var orders = CreateData(count);
            db.AddRange(orders);
            
            db.SaveChanges();
        }

        public static void PrintData()
        {
            using var db = new AppDbContext();

            var orders = db.Orders
                .Include(o => o.Person)
                .Include(o => o.Status)
                .ToList();

            Console.WriteLine("--- Orders ---");
            Console.WriteLine("番号, 担当者, ステータス, 備考, クローズ");
            foreach (var o in orders)
            {
                Console.WriteLine($"{o.Number}, {o.Person?.Name ?? "なし"}, {o.Status?.Text ?? "なし"}, {o.Remarks}, {o.IsClosed}");
            }
            Console.WriteLine("--- /Orders ---");
        }
    }
}