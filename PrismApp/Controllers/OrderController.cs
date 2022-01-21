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

            var results = await db.Orders.Include(o => o.Person)
                                         .Include(o => o.Status)
                                         .ToListAsync();

            return results.Where(filter ?? NoFilter);
        }

        public static async Task<Order?> GetOrderAsync(int id)
        {
            using var db = new AppDbContext();

            var result = await db.Orders.Include(o => o.Person)
                                        .Include(o => o.Status)
                                        .FirstOrDefaultAsync(o => o.Id == id);

            return result;
        }

        public static async Task SaveOrderAsync(Order order)
        {
            using var db = new AppDbContext();

            order.UpdatedDate = DateTime.Now;  // 自動更新されないバグの対応

            db.Update(order);

            await db.SaveChangesAsync();
        }

        public static async Task DeleteOrderAsync(int id)
        {
            using var db = new AppDbContext();

            var target = id > 0 ? await db.Orders.FirstOrDefaultAsync(o => o.Id == id) : null;
            if (target is null)
            {
                return;
            }
            
            db.Remove(target);

            await db.SaveChangesAsync();
        }
    }
}
