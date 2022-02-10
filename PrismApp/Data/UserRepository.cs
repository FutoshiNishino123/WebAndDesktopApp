using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PrismApp.Data
{
    public static class UserRepository
    {
        public static async Task<IEnumerable<User>> List()
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();
                var users = db.Users.Include(u => u.Account)
                                    .OrderBy(u => u.Kana)
                                    .ToList();
                return users;
            });
        }

        public static async Task<User?> GetById(int id)
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();
                var user = db.Users.Include(u => u.Account)
                                   .FirstOrDefault(u => u.Id == id);
                return user;
            });
        }

        public static async Task<User?> GetByAccountNameAndPassword(string accountName, string password)
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();
                var user = db.Users.Include(u => u.Account)
                                   .FirstOrDefault(u => u.Account.Name == accountName && u.Account.Password == password);
                return user;
            });
        }

        public static async Task InsertAsync(User user)
        {
            Debug.Assert(user.Id == 0);

            await Task.Run(() =>
            {
                using var db = new AppDbContext();
                db.Add(user);
                db.SaveChanges();
            });
        }

        public static async Task UpdateAsync(User user)
        {
            Debug.Assert(user.Id != 0);

            await Task.Run(() =>
            {
                using var db = new AppDbContext();
                db.Update(user);
                db.SaveChanges();
            });
        }

        public static async Task DeleteAsync(int id)
        {
            await Task.Run(() =>
            {
                using var db = new AppDbContext();
                var user = db.Users.Include(u => u.Account).FirstOrDefault(u => u.Id == id);

                if (user is null)
                {
                    throw new InvalidOperationException("user not found.");
                }

                if (user.Account is null)
                {
                    throw new InvalidOperationException("user account not found.");
                }

                db.Orders.Where(o => o.User.Id == id).ForEachAsync(o => o.User = null).Wait();
                db.Remove(user);
                db.Remove(user.Account);
                db.SaveChanges();
            });
        }
    }
}
