using Microsoft.EntityFrameworkCore;
using Data;
using Data.Models;

internal static class SampleOrders
{
    private static Person[]? People { get; set; }
    private static Status[]? Statuses { get; set; }

    public static IEnumerable<Order> CreateData(int count)
    {
        var orders = new List<Order>();
        for (var i = 0; i < count; i++)
        {
            var order = new Order
            {
                Id = i + 1,
                CreatedDate = DateTime.Now,
                ExpirationDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Number = "SM" + i.ToString("D4"),
                Person = People?.ElementAtRandom(),
                Status = Statuses?.ElementAtRandom(),
                Remarks = "テスト案件",
                IsClosed = Random.Shared.Next() % 5 == 0,
            };
            orders.Add(order);
        }
        return orders;
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
        var orders = db.Orders.Include(o => o.Person).Include(o => o.Status).ToList();

        Console.WriteLine("--- Orders ---");
        Console.WriteLine("番号, 担当者, ステータス, 備考, クローズ");
        foreach (var o in orders)
        {
            Console.WriteLine($"{o.Number}, {o.Person?.Name ?? "なし"}, {o.Status?.Text ?? "なし"}, {o.Remarks}, {o.IsClosed}");
        }
        Console.WriteLine("--- /Orders ---");
    }
}
