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

            var query = from order in db.Orders
                        select order;

            query = query.Include(o => o.Person)
                         .Include(o => o.Status);

            var results = await query.ToListAsync();

            var filtered = results.Where(filter ?? NoFilter);

            return filtered;
        }

        public static async Task<Order?> GetOrderAsync(int id)
        {
            using var db = new AppDbContext();

            var query = from order in db.Orders
                        select order;

            query = query.Include(o => o.Person)
                         .Include(o => o.Status);

            var result = await query.FirstOrDefaultAsync(o => o.Id == id);

            return result;
        }

        public static async Task SaveOrderAsync(Order order)
        {
            using var db = new AppDbContext();

            // 自動更新されないバグの対応
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
