using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrismApp.Data
{
    public static class OrdersRepository
    {
        public static async Task<IEnumerable<Order>> GetAllAsync(OrderFilter? filter = null)
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();

                var query = from order in db.Orders
                            select order;

                if (filter is not null)
                {
                    query = filter.Apply(query);
                }

                var orders = query
                    .Include(o => o.User)
                    .Include(o => o.Status)
                    .OrderByDescending(o => o.Id)
                    .ToList();

                return orders;
            });
        }

        public static async Task<Order?> FindAsync(int id)
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();

                var order = db.Orders
                    .Include(o => o.User)
                    .Include(o => o.Status)
                    .FirstOrDefault(o => o.Id == id);

                return order;
            });
        }

        public static async Task<string?> GetMaxNumberAsync()
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();

                // NOTE: 0埋め必須 ("1000" < "999"であるため"0999"とする)
                return db.Orders.Max(o => o.Number);
            });
        }

        public static async Task SaveAsync(Order order)
        {
            await Task.Run(() =>
            {
                using var db = new AppDbContext();

                if (db.Orders.Any(o => o.Number == order.Number
                                       && o.Id != order.Id))
                {
                    throw new InvalidOperationException("番号が重複しています。");
                }

                db.Update(order);

                db.SaveChanges();
            });
        }

        public static async Task DeleteAsync(int id)
        {
            await Task.Run(() =>
            {
                using var db = new AppDbContext();

                var order = db.Orders.FirstOrDefault(o => o.Id == id);
                if (order is null)
                {
                    throw new InvalidOperationException("Order was not found.");
                }

                db.Remove(order);

                db.SaveChanges();
            });
        }
    }
}
