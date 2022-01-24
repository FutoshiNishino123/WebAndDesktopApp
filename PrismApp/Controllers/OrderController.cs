using Common.Extensions;
using Common.Utils;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrismApp.Controllers
{
    public static class OrderController
    {
        private static readonly Func<Order, bool> NoFilter = _ => true;

        public static async Task<IEnumerable<Order>> GetOrdersAsync(Func<Order, bool>? filter = null)
        {
            using var db = new AppDbContext();

            var orders = await db.Orders
                .Include(o => o.Person)
                .Include(o => o.Status)
                .ToListAsync();

            var results = orders.Where(filter ?? NoFilter);

            return results;
        }

        public static async Task<Order?> GetOrderAsync(int id)
        {
            using var db = new AppDbContext();

            var order = await db.Orders
                .Include(o => o.Person)
                .Include(o => o.Status)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order;
        }

        public static async Task SaveOrderAsync(Order order)
        {
            using var db = new AppDbContext();

            // NOTE: 自動更新されないバグの対応
            order.UpdatedDate = DateTime.Now;

            db.Update(order);

            await db.SaveChangesAsync();
        }

        public static async Task DeleteOrderAsync(int id)
        {
            using var db = new AppDbContext();

            var target = await db.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (target is null)
            {
                return;
            }

            db.Remove(target);

            await db.SaveChangesAsync();
        }
    }
}
