using Common.Extensions;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrismApp.Data
{
    public static class StatusesRepository
    {
        public static async Task<IEnumerable<Status>> GetAllAsync()
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();

                var statuses = db.Statuses.OrderBy(s => s.Id).ToList();

                return statuses;
            });
        }

        public static async Task<Status?> FindAsync(int id)
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();

                var status = db.Statuses.FirstOrDefault(s => s.Id == id);

                return status;
            });
        }

        public static async Task SaveAsync(Status status)
        {
            await Task.Run(() =>
            {
                using var db = new AppDbContext();

                if (db.Statuses.Any(s => s.Text == status.Text
                                         && s.Id != status.Id))
                {
                    throw new InvalidOperationException("ステータス名が重複しています。");
                }

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
                    throw new InvalidOperationException("Status was not found.");
                }

                // カスケード削除
                db.Orders.Where(o => o.Status.Id == id).ForEachAsync(o => o.Status = null).Wait();

                db.Remove(status);

                db.SaveChanges();
            });
        }
    }
}
