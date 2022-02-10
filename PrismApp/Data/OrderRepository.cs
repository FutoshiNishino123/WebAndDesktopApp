using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PrismApp.Data
{
    public class OrderRepository
    {
        public static async Task<Order?> GetByIdAsync(int id)
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

        public static async Task<IEnumerable<Order>> ListAsync()
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();

                var orders = db.Orders
                    .Include(o => o.User)
                    .Include(o => o.Status)
                    .ToList();

                return orders;
            });
        }

        public static async Task<IEnumerable<Order>> ListAsync(OrderFilter filter)
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();

                var orders = db.Orders
                    .Include(o => o.User)
                    .Include(o => o.Status);

                return filter.Apply(orders).ToList();
            });
        }

        public static async Task<string?> GetMaxNumberAsync()
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();
                return db.Orders.Max(o => o.Number);
            });
        }

        public static async Task InsertAsync(Order order)
        {
            await Task.Run(() =>
            {
                using var db = new AppDbContext();
                db.Add(order);
                db.SaveChanges();
            });
        }

        public static async Task UpdateAsync(Order order)
        {
            await Task.Run(() =>
            {
                using var db = new AppDbContext();
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
                    throw new InvalidOperationException("order not found.");
                }

                db.Remove(order);
                db.SaveChanges();
            });
        }
    }
}
