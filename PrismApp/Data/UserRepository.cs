using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrismApp.Data
{
    public static class UserRepository
    {
        public static async Task<IEnumerable<User>> GetAllAsync()
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

        public static async Task<User?> FindAsync(int id)
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();

                var user = db.Users.Include(u => u.Account)
                                   .FirstOrDefault(u => u.Id == id);

                return user;
            });
        }

        public static async Task<User?> FindAsync(string accountName, string password)
        {
            return await Task.Run(() =>
            {
                using var db = new AppDbContext();

                var user = db.Users.Include(u => u.Account)
                                   .FirstOrDefault(u => u.Account.Name == accountName
                                                        && u.Account.Password == password);

                return user;
            });
        }

        public static async Task SaveAsync(User user)
        {
            if (user.Account is null)
            {
                throw new ArgumentException("アカウントが設定されていません。");
            }

            await Task.Run(() =>
            {
                using var db = new AppDbContext();

                if (db.Users.Any(u => u.Account.Id == user.Account.Id
                                      && u.Id != user.Id))
                {
                    throw new InvalidOperationException("ユーザ アカウントIDが重複しています。");
                }

                // Update だけだと競合が発生するので、条件によってAddと使い分ける
                if (db.Users.Contains(user))
                {
                    db.Update(user);
                }
                else
                {
                    db.Add(user);
                }

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
                    throw new InvalidOperationException("User was not found.");
                }

                if (user.Account is null)
                {
                    throw new InvalidOperationException("Account was not found.");
                }

                // カスケード削除
                db.Orders.Where(o => o.User.Id == id).ForEachAsync(o => o.User = null).Wait();

                db.Remove(user);
                db.Remove(user.Account);

                db.SaveChanges();
            });
        }
    }
}
