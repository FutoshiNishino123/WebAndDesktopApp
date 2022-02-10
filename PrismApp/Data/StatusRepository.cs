using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrismApp.Data
{
    public static class StatusRepository
    {
        public static async Task<IEnumerable<Status>> ListAsync()
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();
                var statuses = db.Statuses.OrderBy(s => s.Id).ToList();
                return statuses;
            });
        }

        public static async Task<Status?> GetByIdAsync(int id)
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();
                var status = db.Statuses.FirstOrDefault(s => s.Id == id);
                return status;
            });
        }

        public static async Task InsertAsync(Status status)
        {
            await Task.Run(() =>
            {
                using var db = new AppDbContext();
                db.Add(status);
                db.SaveChanges();
            });
        }

        public static async Task UpdateAsync(Status status)
        {
            await Task.Run(() =>
            {
                using var db = new AppDbContext();
                db.Update(status);
                db.SaveChanges();
            });
        }

        public static async Task DeleteAsync(int id)
        {
            await Task.Run(() =>
            {
                using var db = new AppDbContext();
                var status = db.Statuses.FirstOrDefault(s => s.Id == id);
                if (status == null)
                {
                    throw new InvalidOperationException("status not found.");
                }

                db.Orders.Where(o => o.Status.Id == id).ForEachAsync(o => o.Status = null).Wait();
                db.Remove(status);
                db.SaveChanges();
            });
        }
    }
}
