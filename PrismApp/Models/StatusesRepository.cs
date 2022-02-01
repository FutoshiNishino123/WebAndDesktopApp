using Common.Extensions;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrismApp.Models
{
    public static class StatusesRepository
    {
        public static async Task<IEnumerable<Status>> GetStatusesAsync()
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();

                var statuses = db.Statuses.OrderBy(s => s.Id).ToList();

                return statuses;
            });
        }

        public static async Task<Status?> FindStatusAsync(int id)
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();

                var status = db.Statuses.FirstOrDefault(s => s.Id == id);

                return status;
            });
        }

        public static async Task SaveStatusAsync(Status status)
        {
            await Task.Run(() =>
            {
                using var db = new AppDbContext();

                db.Update(status);

                db.SaveChanges();
            });
        }

        public static async Task DeleteStatusAsync(int id)
        {
            await Task.Run(() =>
            {
                using var db = new AppDbContext();

                var status = db.Statuses.FirstOrDefault(s => s.Id == id);
                if (status == null)
                {
                    throw new InvalidOperationException($"Status (id:{id}) was not found.");
                }

                // 外部キーを削除
                db.Orders.Where(o => o.Status.Id == id).ForEachAsync(o => o.Status = null).Wait();

                db.Remove(status);

                db.SaveChanges();
            });
        }

        public static async Task<bool> Contains(int id)
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();

                return db.Statuses.Any(s => s.Id == id);
            });
        }

        public static async Task<bool> Contains(string text)
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();

                return db.Statuses.Any(s => s.Text == text);
            });
        }
    }
}
