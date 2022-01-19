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

            Order? target = null;
            if (order.Id > 0)
            {
                target = await db.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
            }

            if (target is null)
            {
                target = new Order();
                db.Add(target);
            }
            
            order.CopyPropertiesTo(target);

    #region 一時的な措置
            // TODO 更新日時の自動更新
            target.UpdatedDate = DateTime.Now;
    #endregion

            await db.SaveChangesAsync();
        }

        public static async Task DeleteOrderAsync(int id)
        {
            using var db = new AppDbContext();

            Order? target = null;
            if (id > 0)
            {
                target = await db.Orders.FirstOrDefaultAsync(o => o.Id == id);
            }

            if (target is null)
            {
                throw new InvalidOperationException("レコードが見つかりません。");
            }
            
            db.Remove(target);

            await db.SaveChangesAsync();
        }
    }
}
