using Common.Extensions;
using Common.Utils;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrismApp.Models
{
    public static class UsersRepository
    {
        public static async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();
                var users = db.Users.Include(u => u.Account)
                                    .OrderBy(u => u.Id)
                                    .ToList();
                return users;
            });
        }

        public static async Task<User?> FindUserAsync(int id)
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();
                var user = db.Users.Include(u => u.Account)
                                   .FirstOrDefault(u => u.Id == id);
                return user;
            });
        }

        public static async Task<User?> FindUserAsync(string accountId, string password)
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();
                var user = db.Users.Include(u => u.Account)
                                   .FirstOrDefault(u => u.Account.Id == accountId
                                                        && u.Account.Password == password);
                return user;
            });
        }

        public static async Task SaveUserAsync(User user)
        {
            await Task.Run(() =>
            {
                using var db = new AppDbContext();

                if (db.Users.Any(u => u.Id == user.Id))
                {
                    db.Users.Update(user);
                }
                else
                {
                    db.Users.Add(user);
                }

                db.SaveChanges();
            });
        }

        public static async Task DeleteUserAsync(int id)
        {
            await Task.Run(() =>
            {
                using var db = new AppDbContext();

                var user = db.Users.Include(u => u.Account).FirstOrDefault(u => u.Id == id);
                if (user is null)
                {
                    throw new InvalidOperationException("削除する対象が見つかりません。");
                }

                if (user.Account is not null)
                {
                    db.Remove(user.Account);
                }

                db.Remove(user);

                // 参照を解除
                db.Orders.Where(o => o.User.Id == id).ForEachAsync(o => o.User = null).Wait();

                db.SaveChanges();
            });
        }
    }
}
